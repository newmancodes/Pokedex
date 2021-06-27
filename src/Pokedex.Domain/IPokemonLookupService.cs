using System.Threading;
using System.Threading.Tasks;

namespace Pokedex.Domain
{
    public interface IPokemonLookupService
    {
        Task<Pokemon> ByName(string name, CancellationToken cancellationToken);
    }
}