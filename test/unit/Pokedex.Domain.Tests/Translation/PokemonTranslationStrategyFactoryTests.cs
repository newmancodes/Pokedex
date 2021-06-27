using System.Runtime.InteropServices;
using FluentAssertions;
using Pokedex.Domain.Translation;
using Xunit;

namespace Pokedex.Domain.Tests.Translation
{
    public class PokemonTranslationStrategyFactoryTests
    {
        [Fact]
        public void Legendary_Pokemon_Are_Translated_By_Yoda()
        {
            // Arrange
            var legendaryPokemon = new Pokemon("some_name", "some_description", "some_habitat", true);
            var sut = new PokemonTranslationStrategyFactory();

            // Act
            var translationStrategy = sut.For(legendaryPokemon);

            // Assert
            translationStrategy.Should().BeOfType<YodaTranslationStrategy>();
        }
        
        [InlineData("cave")]
        [InlineData("CAVE")]
        [Theory]
        public void Cave_Dwelling_Pokemon_Are_Translated_By_Yoda(string habitat)
        {
            // Arrange
            var legendaryPokemon = new Pokemon("some_name", "some_description", habitat, false);
            var sut = new PokemonTranslationStrategyFactory();

            // Act
            var translationStrategy = sut.For(legendaryPokemon);

            // Assert
            translationStrategy.Should().BeOfType<YodaTranslationStrategy>();
        }
        
        [Fact]
        public void Basic_Pokemon_Are_Translated_By_Shakespeare()
        {
            // Arrange
            var legendaryPokemon = new Pokemon("some_name", "some_description", "some_habitat", false);
            var sut = new PokemonTranslationStrategyFactory();

            // Act
            var translationStrategy = sut.For(legendaryPokemon);

            // Assert
            translationStrategy.Should().BeOfType<ShakespeareTranslationStrategy>();
        }
    }
}