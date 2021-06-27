using System.Threading;
using System.Threading.Tasks;

namespace Pokedex.Domain.Translation
{
    public interface ITranslationService
    {
        Task<string> Translate(string value, string intoLanguage, CancellationToken cancellationToken);
    }
}