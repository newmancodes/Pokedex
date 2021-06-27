using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Pokedex.Domain;
using Pokedex.Domain.Translation;

namespace Pokedex.Application.Pokemon
{
    public class TranslateNamedPokemon : IRequestHandler<TranslateNamedPokemonQuery, Result<Domain.Pokemon>>
    {
        private readonly IPokemonLookupService pokemonLookupService;
        private readonly ITranslationStrategyFactory<Domain.Pokemon> translationStrategyFactory;

        public TranslateNamedPokemon(
            IPokemonLookupService pokemonLookupService,
            ITranslationStrategyFactory<Domain.Pokemon> translationStrategyFactory)
        {
            this.pokemonLookupService = pokemonLookupService;
            this.translationStrategyFactory = translationStrategyFactory;
        }
        public async Task<Result<Domain.Pokemon>> Handle(TranslateNamedPokemonQuery request, CancellationToken cancellationToken)
        {
            var pokemon = await this.pokemonLookupService.ByName(request.Name, cancellationToken);
            
            if (pokemon == null)
            {
                return Result<Domain.Pokemon>.Unsuccessful;
            }

            var translationStrategy = this.translationStrategyFactory.For(pokemon);
            await pokemon.Translate(translationStrategy, cancellationToken);
            return Result<Domain.Pokemon>.Successful(pokemon);
        }
    }

    public record TranslateNamedPokemonQuery(string Name) : IRequest<Result<Domain.Pokemon>>;
}