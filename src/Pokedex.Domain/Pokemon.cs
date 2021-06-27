using System;
using System.Threading;
using System.Threading.Tasks;
using Pokedex.Domain.Translation;

namespace Pokedex.Domain
{
    public class Pokemon
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        public string Habitat { get; private set; }

        public bool IsLegendary { get; private set; }
        
        public Pokemon(string name, string description, string habitat, bool isLegendary)
        {
            Name = name;
            Description = description;
            Habitat = habitat;
            IsLegendary = isLegendary;
        }

        public bool HasHabitat(string habitat)
        {
            return string.Equals(this.Habitat, habitat, StringComparison.OrdinalIgnoreCase);
        }

        public async Task Translate(ITranslationStrategy translationStrategy, CancellationToken cancellationToken)
        {
            var translatedDescription = await translationStrategy.Translate(this.Description, cancellationToken);
            if (translatedDescription.WasSuccessful)
            {
                this.Description = translatedDescription.Value;
            }
        }
    }
}