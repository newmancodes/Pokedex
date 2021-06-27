using System.Threading;
using System.Threading.Tasks;

namespace Pokedex.Domain.Translation
{
    public class ShakespeareTranslationStrategy : ITranslationStrategy
    {
        public Task<Result<string>> Translate(string value, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}