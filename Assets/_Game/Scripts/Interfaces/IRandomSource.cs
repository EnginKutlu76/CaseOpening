namespace CaseOpening.Interfaces
{
    // Abstracts randomness so gameplay services can be tested with deterministic values.
    public interface IRandomSource
    {
        float Value { get; }
        int Range(int minInclusive, int maxExclusive);
    }
}
