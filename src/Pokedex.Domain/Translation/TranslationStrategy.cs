using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Pokedex.Domain.Translation
{
    internal class TranslationStrategy : ITranslationStrategy
    {
        private readonly ITranslationService translationService;
        private readonly string intoLanguage;
        private readonly ILogger<TranslationStrategy> logger;

        internal TranslationStrategy(ITranslationService translationService, string intoLanguage, ILoggerFactory loggerFactory)
        {
            this.translationService = translationService;
            this.intoLanguage = intoLanguage;
            this.logger = loggerFactory.CreateLogger<TranslationStrategy>();

        }
        
        public async Task<Result<string>> Translate(string value, CancellationToken cancellationToken)
        {
            try
            {
                var translationResult = await this.translationService.Translate(value, this.intoLanguage, cancellationToken);
                return Result<string>.Successful(translationResult);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"Unable to translate {value} into {this.intoLanguage}.");
                return Result<string>.Unsuccessful;
            }
        }
    }
}