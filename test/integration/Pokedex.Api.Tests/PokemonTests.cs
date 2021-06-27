using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
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

        [Fact]
        public async Task A_Legendary_Pokemon_IsFound()
        {
            // Arrange
            using var factory = GetConfiguredFactory();
            using var client = factory.CreateClient();

            // Act
            var result = await client.GetFromJsonAsync<Models.Pokemon>("/pokemon/mewtwo");

            // Assert
            result.Should().NotBeNull();
            var expectedPokemon = new Models.Pokemon(
                "mewtwo",
                "It was created by\na scientist after\nyears of horrific\fgene splicing and\nDNA engineering\nexperiments.",
                "rare",
                true);
            result.Should().BeEquivalentTo(expectedPokemon);
        }

        [Fact]
        public async Task Handle_Potentially_Missing_Pokemon_Properties()
        {
            // Arrange
            using var factory = GetConfiguredFactory();
            using var client = factory.CreateClient();

            // Act
            var result = await client.GetFromJsonAsync<Models.Pokemon>("/pokemon/some_minimal_pokemon");

            // Assert
            result.Should().NotBeNull();
            var expectedPokemon = new Models.Pokemon(
                "some_minimal_pokemon",
                null,
                null,
                false);
            result.Should().BeEquivalentTo(expectedPokemon);
        }

        [Fact]
        public async Task An_Unknown_Pokemon_Can_Not_Be_Translated()
        {
            // Arrange
            using var factory = GetConfiguredFactory();
            using var client = factory.CreateClient();
            
            // Act
            var result = await client.GetAsync("/pokemon/translated/some_unknown_pokemon");
            
            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task A_Legendary_Pokemon_Is_Described_By_Yoda()
        {
            // Arrange
            using var factory = GetConfiguredFactory();
            using var client = factory.CreateClient();

            // Act
            var result = await client.GetFromJsonAsync<Models.Pokemon>("/pokemon/translated/mewtwo");

            // Assert
            result.Should().NotBeNull();
            var expectedPokemon = new Models.Pokemon(
                "mewtwo",
                "Created by\na scientist after\nyears of horrific\fgene splicing and\ndna engineering\nexperiments, it was.",
                "rare",
                true);
            result.Should().BeEquivalentTo(expectedPokemon);
        }
    }
}