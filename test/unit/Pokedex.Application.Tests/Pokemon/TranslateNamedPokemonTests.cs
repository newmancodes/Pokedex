using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Pokedex.Application.Pokemon;
using Pokedex.Domain;
using Pokedex.Domain.Translation;
using Xunit;

namespace Pokedex.Application.Tests.Pokemon
{
    public class TranslateNamedPokemonTests
    {
        [Fact]
        public async Task Unsuccessful_Lookup_Returns_No_Pokemon()
        {
            // Arrange
            using var cancellationTokenSource = new CancellationTokenSource();
            var pokemonName = "some_name";
            var request = new TranslateNamedPokemonQuery(pokemonName);
            Domain.Pokemon retrievedPokemon = null;
            var lookupService = Substitute.For<IPokemonLookupService>();
            lookupService.ByName(pokemonName, cancellationTokenSource.Token).Returns(retrievedPokemon);
            var translationStrategyFactory = Substitute.For<ITranslationStrategyFactory<Domain.Pokemon>>();
            var sut = new TranslateNamedPokemon(lookupService, translationStrategyFactory);
            
            // Act
            var result = await sut.Handle(request, cancellationTokenSource.Token);

            // Assert
            result.WasSuccessful.Should().BeFalse();
            result.Value.Should().BeNull();
        }

        [Fact]
        public async Task Successful_Lookup_Returns_Translated_Pokemon()
        {
            // Arrange
            using var cancellationTokenSource = new CancellationTokenSource();
            var pokemonName = "some_name";
            var request = new TranslateNamedPokemonQuery(pokemonName);
            var description = "some_description";
            var retrievedPokemon = new Domain.Pokemon(pokemonName, description, "some_habitat", false);
            var lookupService = Substitute.For<IPokemonLookupService>();
            lookupService.ByName(pokemonName, cancellationTokenSource.Token).Returns(retrievedPokemon);

            var translationStrategyFactory = Substitute.For<ITranslationStrategyFactory<Domain.Pokemon>>();
            var translationStrategy = Substitute.For<ITranslationStrategy>();
            var translatedDescription = "some_translated_description";
            translationStrategy.Translate(description, cancellationTokenSource.Token).Returns(Result<string>.Successful(translatedDescription));
            translationStrategyFactory.For(retrievedPokemon).Returns(translationStrategy);
            var sut = new TranslateNamedPokemon(lookupService, translationStrategyFactory);
            
            // Act
            var result = await sut.Handle(request, cancellationTokenSource.Token);

            // Assert
            result.WasSuccessful.Should().BeTrue();
            var translatedPokemon = new Domain.Pokemon(pokemonName, translatedDescription, "some_habitat", false);
            result.Value.Should().BeEquivalentTo(translatedPokemon);
        }
    }
}