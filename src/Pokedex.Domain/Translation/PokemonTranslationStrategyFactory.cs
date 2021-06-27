using Microsoft.Extensions.Logging;

namespace Pokedex.Domain.Translation
{
    public class PokemonTranslationStrategyFactory : ITranslationStrategyFactory<Pokemon>
    {
        private const string CaveDwellingHabitatName = "cave";
        private readonly ITranslationService translationService;
        private readonly ILoggerFactory loggerFactory;

        public PokemonTranslationStrategyFactory(
            ITranslationService translationService,
            ILoggerFactory loggerFactory)
        {
            this.translationService = translationService;
            this.loggerFactory = loggerFactory;
        }

        public ITranslationStrategy For(Pokemon item)
        {
            if (item.IsLegendary
                || item.HasHabitat(CaveDwellingHabitatName))
            {
                return new YodaTranslationStrategy(this.translationService, this.loggerFactory);
            }

            return new ShakespeareTranslationStrategy(this.translationService, this.loggerFactory);
        }
    }
}