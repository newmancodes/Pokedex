using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Pokedex.Domain;

namespace Pokedex.Application.Pokemon
{
    public class FindPokemonByName : IRequestHandler<FindPokemonByNameQuery, Result<Domain.Pokemon>>
    {
        private readonly IPokemonLookupService pokemonLookupService;

        public FindPokemonByName(IPokemonLookupService pokemonLookupService)
        {
            this.pokemonLookupService = pokemonLookupService;
        }

        public async Task<Result<Domain.Pokemon>> Handle(FindPokemonByNameQuery request, CancellationToken cancellationToken)
        {
            var pokemon = await this.pokemonLookupService.ByName(request.Name, cancellationToken);
            
            if (pokemon == null)
            {
                return Result<Domain.Pokemon>.Unsuccessful;
            }

            return Result<Domain.Pokemon>.Successful(pokemon);
        }
    }

    public record FindPokemonByNameQuery(string Name) : IRequest<Result<Domain.Pokemon>>;
}