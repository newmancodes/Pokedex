using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Pokedex.Domain.Translation;
using Pokedex.Infrastructure.FunTranslation.DTO;

namespace Pokedex.Infrastructure.FunTranslation
{
    public class FunTranslationService : ITranslationService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public FunTranslationService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        
        public async Task<string> Translate(string value, string intoLanguage, CancellationToken cancellationToken)
        {
            using var httpClient = this.httpClientFactory.CreateClient("funtranslation");
            var response = await httpClient.PostAsJsonAsync(
                $"/translate/{intoLanguage}/",
                new { Text = value },
                cancellationToken);

            var translationResult = await response.Content.ReadFromJsonAsync<FunTranslationResult>(cancellationToken: cancellationToken);
            return translationResult.Contents.Translated;
        }
    }
}