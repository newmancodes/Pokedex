using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Pokedex.Domain;
using Pokedex.Infrastructure.PokeApi.DTO;

namespace Pokedex.Infrastructure.PokeApi
{
    public class PokemonLookupService : IPokemonLookupService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public PokemonLookupService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<Pokemon> ByName(string name, CancellationToken cancellationToken)
        {
            using var httpClient = this.httpClientFactory.CreateClient("pokeapi");
            var response = await httpClient.GetAsync($"/api/v2/pokemon-species/{name}/", cancellationToken);

            Pokemon pokemon = null;

            if (response.IsSuccessStatusCode)
            {
                var pokemonResult = await response.Content.ReadFromJsonAsync<PokemonSpeciesResult>(cancellationToken: cancellationToken);
                var firstEnglishFlavourText = pokemonResult.FlavourTexts?.FirstOrDefault(ft => ft.Language.Name == "en")?.Text;
                pokemon = new Pokemon(
                    pokemonResult.Name,
                    firstEnglishFlavourText,
                    pokemonResult.Habitat?.Name,
                    pokemonResult.IsLegendary);
            }
            
            return pokemon;
        }
    }
}