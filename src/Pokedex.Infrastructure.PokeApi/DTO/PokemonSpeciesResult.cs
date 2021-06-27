using System.Text.Json.Serialization;

namespace Pokedex.Infrastructure.PokeApi.DTO
{
    public class PokemonSpeciesResult
    {
        public string Name { get; set; }
        
        [JsonPropertyName("flavor_text_entries")]
        public FlavourText[] FlavourTexts { get; set; }
        
        public Habitat Habitat { get; set; }
        
        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }
    }
}