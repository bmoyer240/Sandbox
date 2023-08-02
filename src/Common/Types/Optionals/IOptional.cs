namespace Application;

public interface IMaybe
{
    bool HasValue   { get; }
    bool HasNoValue { get; }
}

public interface IOptional<out T> : IMaybe
{
    T Value { get; }
}
