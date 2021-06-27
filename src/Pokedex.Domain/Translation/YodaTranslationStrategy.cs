using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Pokedex.Domain.Translation
{
    public class YodaTranslationStrategy : ITranslationStrategy
    {
        private readonly ITranslationStrategy translationStrategy;

        public YodaTranslationStrategy(
            ITranslationService translationService,
            ILoggerFactory loggerFactory)
        {
            this.translationStrategy = new TranslationStrategy(translationService, "yoda", loggerFactory);
        }

        public Task<Result<string>> Translate(string value, CancellationToken cancellationToken)
        {
            return this.translationStrategy.Translate(value, cancellationToken);
        }
    }
}