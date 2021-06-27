using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Pokedex.Domain.Translation
{
    public class ShakespeareTranslationStrategy : ITranslationStrategy
    {
        private readonly ITranslationService translationService;
        private readonly ILogger<ShakespeareTranslationStrategy> logger;

        public ShakespeareTranslationStrategy(
            ITranslationService translationService,
            ILoggerFactory loggerFactory)
        {
            this.translationService = translationService;
            this.logger = loggerFactory.CreateLogger<ShakespeareTranslationStrategy>();
        }

        public async Task<Result<string>> Translate(string value, CancellationToken cancellationToken)
        {
            try
            {
                var translationResult = await this.translationService.Translate(value, "shakespeare", cancellationToken);
                return Result<string>.Successful(translationResult);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"Unable to translate {value} into Shakespeare.");
                return Result<string>.Unsuccessful;
            }
        }
    }
}