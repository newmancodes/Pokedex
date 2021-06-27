using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Pokedex.Api.Tests
{
    public class PokemonTests
    {
        private WebApplicationFactory<Startup> GetConfiguredFactory()
        {
            return new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, configBuilder) =>
                {
                    configBuilder.AddInMemoryCollection(
                        new Dictionary<string, string>()
                        {
                            { "PokeApi:BaseAddress", "http://localhost:8080/" }
                        });
                });
            });
        }

        [Fact]
        public async Task An_Unknown_Pokemon_Is_Not_Found()
        {
            // Arrange
            using var factory = GetConfiguredFactory();
            using var client = factory.CreateClient();
            
            // Act
            var result = await client.GetAsync("/pokemon/some_unknown_pokemon");
            
            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task A_Known_Pokemon_Is_Found()
        {
            // Arrange
            using var factory = GetConfiguredFactory();
            using var client = factory.CreateClient();

            // Act
            var result = await client.GetFromJsonAsync<Models.Pokemon>("/pokemon/squirtle");

            // Assert
            result.Should().NotBeNull();
            var expectedPokemon = new Models.Pokemon(
                "squirtle",
                "Shoots water at\nprey while in the\nwater.\fWithdraws into\nits shell when in\ndanger.",
                "waters-edge",
                false);
            result.Should().BeEquivalentTo(expectedPokemon);
        }
    }
}