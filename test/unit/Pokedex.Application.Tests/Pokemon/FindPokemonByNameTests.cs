using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Pokedex.Application.Pokemon;
using Pokedex.Domain;
using Xunit;

namespace Pokedex.Application.Tests.Pokemon
{
    public class FindPokemonByNameTests
    {
        [Fact]
        public async Task Unsuccessful_Lookup_Returns_No_Pokemon()
        {
            // Arrange
            using var cancellationTokenSource = new CancellationTokenSource();
            var pokemonName = "some_name";
            var request = new FindPokemonByNameQuery(pokemonName);
            Domain.Pokemon retrievedPokemon = null;
            var lookupService = Substitute.For<IPokemonLookupService>();
            lookupService.ByName(pokemonName, cancellationTokenSource.Token).Returns(retrievedPokemon);
            var sut = new FindPokemonByName(lookupService);
            
            // Act
            var result = await sut.Handle(request, cancellationTokenSource.Token);

            // Assert
            result.WasSuccessful.Should().BeFalse();
            result.Value.Should().BeNull();
        }
        
        [Fact]
        public async Task Successful_Lookup_Returns_Correct_Pokemon()
        {
            // Arrange
            using var cancellationTokenSource = new CancellationTokenSource();
            var pokemonName = "some_name";
            var request = new FindPokemonByNameQuery(pokemonName);
            var retrievedPokemon = new Domain.Pokemon(pokemonName, "some_description", "some_habitat", false);
            var lookupService = Substitute.For<IPokemonLookupService>();
            lookupService.ByName(pokemonName, cancellationTokenSource.Token).Returns(retrievedPokemon);
            var sut = new FindPokemonByName(lookupService);
            
            // Act
            var result = await sut.Handle(request, cancellationTokenSource.Token);

            // Assert
            result.WasSuccessful.Should().BeTrue();
            result.Value.Should().BeSameAs(retrievedPokemon);
        }
    }
}