using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Pokedex.Domain.Translation
{
    public class ShakespeareTranslationStrategy : ITranslationStrategy
    {
        private readonly ITranslationStrategy translationStrategy;

        public ShakespeareTranslationStrategy(
            ITranslationService translationService,
            ILoggerFactory loggerFactory)
        {
            this.translationStrategy = new TranslationStrategy(translationService, "shakespeare", loggerFactory);
        }

        public Task<Result<string>> Translate(string value, CancellationToken cancellationToken)
        {
            return this.translationStrategy.Translate(value, cancellationToken);
        }
    }
}