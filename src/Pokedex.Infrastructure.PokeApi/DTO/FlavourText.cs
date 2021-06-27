using System.Text.Json.Serialization;

namespace Pokedex.Infrastructure.PokeApi.DTO
{
    public class FlavourText
    {
        public Language Language { get; set; }
        
        [JsonPropertyName("flavor_text")]
        public string Text { get; set; }
    }
}