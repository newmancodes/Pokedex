using System;
using FluentAssertions;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;
using Xunit;

namespace Pokedex.Api.LoadTest
{
    public class PokemonTests
    {
        [Fact]
        [Trait("Category", "LoadTest")]
        public void Retrievals_Are_Quick_Enough()
        {
            // Arrange
            var step = Step.Create(
                "fetch_squirtle_information",
                HttpClientFactory.Create(),
                context =>
                {
                    var request = Http.CreateRequest("GET", "http://localhost:5000/pokemon/squirtle");

                    return Http.Send(request, context);
                },
                timeout: TimeSpan.FromSeconds(2));

            var scenario = ScenarioBuilder
                .CreateScenario("fetch_squirtle_information", step)
                .WithWarmUpDuration(TimeSpan.FromSeconds(10))
                .WithLoadSimulations(
                    Simulation.RampConstant(50, TimeSpan.FromSeconds(30)),
                    Simulation.RampPerSec(5, TimeSpan.FromSeconds(30)));

            // Act
            var stats = NBomberRunner.RegisterScenarios(scenario).Run();

            // Assert
            stats.ScenarioStats[0].StepStats[0].Fail.Request.Count.Should().Be(0);
            stats.ScenarioStats[0].StepStats[0].Ok.Latency.Percent95.Should().BeLessOrEqualTo(750);
        }
        
        [Fact]
        [Trait("Category", "LoadTest")]
        public void Translations_Are_Quick_Enough()
        {
            // Arrange
            var step = Step.Create(
                "fetch_translated_squirtle_information",
                HttpClientFactory.Create(),
                context =>
                {
                    var request = Http.CreateRequest("GET", "http://localhost:5000/pokemon/translated/squirtle");

                    return Http.Send(request, context);
                },
                timeout: TimeSpan.FromSeconds(2));

            var scenario = ScenarioBuilder
                .CreateScenario("fetch_translated_squirtle_information", step)
                .WithWarmUpDuration(TimeSpan.FromSeconds(10))
                .WithLoadSimulations(
                    Simulation.RampConstant(50, TimeSpan.FromSeconds(30)),
                    Simulation.RampPerSec(5, TimeSpan.FromSeconds(30)));

            // Act
            var stats = NBomberRunner.RegisterScenarios(scenario).Run();

            // Assert
            stats.ScenarioStats[0].StepStats[0].Fail.Request.Count.Should().Be(0);
            stats.ScenarioStats[0].StepStats[0].Ok.Latency.Percent95.Should().BeLessOrEqualTo(1500);
        }
    }
}