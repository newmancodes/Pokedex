using System.Threading;
using System.Threading.Tasks;

namespace Pokedex.Domain.Translation
{
    public interface ITranslationStrategy
    {
        Task<Result<string>> Translate(string value, CancellationToken cancellationToken);
    }
}