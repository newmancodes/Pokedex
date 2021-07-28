﻿using System;
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
        public void Translations_Are_Quick_Enough()
        {
            // Arrange
            var step = Step.Create(
                "fetch_squirtle_information",
                HttpClientFactory.Create(),
                context =>
                {
                    var request = Http.CreateRequest("GET", "http://localhost:5000/pokemon/squirtle");

                    return Http.Send(request, context);
                });

            var scenario = ScenarioBuilder
                .CreateScenario("fetch_squirtle_information", step)
                .WithWarmUpDuration(TimeSpan.FromSeconds(10))
                .WithLoadSimulations(
                    Simulation.RampConstant(100, TimeSpan.FromSeconds(30)),
                    Simulation.RampPerSec(5, TimeSpan.FromSeconds(30)));

            // Act
            var stats = NBomberRunner.RegisterScenarios(scenario).Run();

            // Assert
            stats.ScenarioStats[0].StepStats[0].Ok.Latency.Percent95.Should().BeLessOrEqualTo(750);
        }
    }
}