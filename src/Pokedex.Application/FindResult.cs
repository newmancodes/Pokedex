namespace Pokedex.Application
{
    public class Result<T>
    {
        public bool WasSuccessful { get; }

        public T Value { get; }

        private Result(bool wasSuccessful, T value)
        {
            WasSuccessful = wasSuccessful;
            Value = value;
        }

        public static Result<T> Successful(T value) => new(true, value);

        public static Result<T> Unsuccessful => new(false, default);
    }
}