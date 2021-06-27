namespace Pokedex.Domain.Translation
{
    public interface ITranslationStrategyFactory<T>
    {
        ITranslationStrategy For(T item);
    }
}