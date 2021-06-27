using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Pokedex.Domain.Translation
{
    public class YodaTranslationStrategy : ITranslationStrategy
    {
        private readonly ITranslationService translationService;
        private readonly ILogger<YodaTranslationStrategy> logger;

        public YodaTranslationStrategy(
            ITranslationService translationService,
            ILoggerFactory loggerFactory)
        {
            this.translationService = translationService;
            this.logger = loggerFactory.CreateLogger<YodaTranslationStrategy>();
        }

        public async Task<Result<string>> Translate(string value, CancellationToken cancellationToken)
        {
            try
            {
                var translationResult = await this.translationService.Translate(value, "yoda", cancellationToken);
                return Result<string>.Successful(translationResult);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"Unable to translate {value} into Yoda.");
                return Result<string>.Unsuccessful;
            }
        }
    }
}