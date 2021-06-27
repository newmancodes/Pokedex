using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Pokedex.Domain.Translation;
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

        [Fact]
        public async Task Pokemon_Can_Be_Translated()
        {
            // Arrange
            using var cancellationTokenSource = new CancellationTokenSource();
            var name = "some_name";
            var description = "some_description";
            var habitat = "habitat";
            var isLegendary = true;
            var pokemon = new Pokemon(name, description, habitat, isLegendary);
            var translationStrategy = Substitute.For<ITranslationStrategy>();
            var translatedDescription = "some_translated_description";
            translationStrategy.Translate(description, cancellationTokenSource.Token).Returns(Result<string>.Successful(translatedDescription));

            // Act
            await pokemon.Translate(translationStrategy, cancellationTokenSource.Token);

            // Assert
            pokemon.Name.Should().Be(name);
            pokemon.Description.Should().Be(translatedDescription);
            pokemon.Habitat.Should().Be(habitat);
            pokemon.IsLegendary.Should().Be(isLegendary);
        }

        [Fact]
        public async Task Pokemon_Translation_Failures_Maintain_Original_Information()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            var name = "some_name";
            var description = "some_description";
            var habitat = "habitat";
            var isLegendary = true;
            var pokemon = new Pokemon(name, description, habitat, isLegendary);
            var translationStrategy = Substitute.For<ITranslationStrategy>();
            translationStrategy.Translate(description, cancellationTokenSource.Token).Returns(Result<string>.Unsuccessful);

            // Act
            await pokemon.Translate(translationStrategy, cancellationTokenSource.Token);

            // Assert
            pokemon.Name.Should().Be(name);
            pokemon.Description.Should().Be(description);
            pokemon.Habitat.Should().Be(habitat);
            pokemon.IsLegendary.Should().Be(isLegendary);
        }
    }
}