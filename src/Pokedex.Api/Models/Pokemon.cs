namespace Pokedex.Api.Models
{
    public record Pokemon
    {
        public string Name { get; init; }
        
        public string Description { get; init; }
        
        public string Habitat { get; init; }
        
        public bool IsLegendary { get; init; }

        public Pokemon()
        {
        }

        public Pokemon(string name, string description, string habitat, bool isLegendary)
            : this()
        {
            Name = name;
            Description = description;
            Habitat = habitat;
            IsLegendary = isLegendary;
        }

        public Pokemon(Domain.Pokemon pokemon)
            : this(pokemon.Name, pokemon.Description, pokemon.Habitat, pokemon.IsLegendary)
        {
        }
    }
}