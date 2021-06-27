using FluentAssertions;
using Xunit;

namespace Pokedex.Domain.Tests
{
    public class PokemonTests
    {
        [Fact]
        public void Pokemon_Can_Be_Created()
        {
            // Arrange
            var name = "some_name";
            var description = "some_description";
            var habitat = "habitat";
            var isLegendary = true;
            
            // Act
            var pokemon = new Pokemon(name, description, habitat, isLegendary);

            // Assert
            pokemon.Name.Should().Be(name);
            pokemon.Description.Should().Be(description);
            pokemon.Habitat.Should().Be(habitat);
            pokemon.IsLegendary.Should().Be(isLegendary);
        }
    }
}