namespace Pokedex.Domain.Translation
{
    public class PokemonTranslationStrategyFactory : ITranslationStrategyFactory<Pokemon>
    {
        private const string CaveDwellingHabitatName = "cave";

        public ITranslationStrategy For(Pokemon item)
        {
            if (item.IsLegendary
                || item.HasHabitat(CaveDwellingHabitatName))
            {
                return new YodaTranslationStrategy();
            }

            return new ShakespeareTranslationStrategy();
        }
    }
}