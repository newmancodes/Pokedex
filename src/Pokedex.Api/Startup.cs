using System;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Pokedex.Application;
using Pokedex.Domain;
using Pokedex.Domain.Translation;
using Pokedex.Infrastructure.FunTranslation;
using Pokedex.Infrastructure.PokeApi;
using Polly;
using Polly.Extensions.Http;

namespace Pokedex.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.environment = environment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ITranslationStrategyFactory<Pokemon>, PokemonTranslationStrategyFactory>();
            services.AddScoped<IPokemonLookupService, PokemonLookupService>();
            services.AddScoped<ITranslationService, FunTranslationService>();

            services.AddOpenTelemetryTracing(builder =>
            {
                builder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddJaegerExporter(options =>
                    {
                        options.AgentHost = Configuration.GetValue<string>("Jaeger:AgentHost");
                    }); // Use OTLP exporter preferably. 

                if (this.environment.IsDevelopment())
                {
                    builder.AddConsoleExporter();
                }

                builder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Pokedex.Api"));
            });

            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(retryAttempt * 200)); // exponential back off would be better, consider circuit breaker

            services
                .AddHttpClient("pokeapi", c => 
                {
                    c.BaseAddress = new Uri(Configuration.GetValue<string>("PokeApi:BaseAddress"));
                })
                .AddPolicyHandler(retryPolicy);

            services
                .AddHttpClient("funtranslation", c => 
                {
                    c.BaseAddress = new Uri(Configuration.GetValue<string>("FunTranslation:BaseAddress"));
                })
                .AddPolicyHandler(retryPolicy);

            services.AddMediatR(typeof(ActivityBehaviour<,>).Assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ActivityBehaviour<,>));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Pokedex.Api", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokedex.Api v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}