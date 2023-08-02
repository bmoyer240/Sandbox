namespace Application.Common.Types.Maybe.Comparers;

internal sealed class OptionalEqualityComparer<T> : IEqualityComparer<Optional<T>>
{
    private readonly IEqualityComparer<T> _equalityComparer;

    public OptionalEqualityComparer(IEqualityComparer<T>? equalityComparer = null)
    {
        _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
    }

    public bool Equals(Optional<T> x, Optional<T> y)
    {
        if (x.HasNoValue && y.HasNoValue)
        {
            return true;
        }

        return x.HasValue
               && y.HasValue
               && _equalityComparer.Equals(x.Value, y.Value);
    }

    public int GetHashCode(Optional<T> obj) =>
        obj.HasNoValue ? 0 : _equalityComparer.GetHashCode(obj.Value!);
}
