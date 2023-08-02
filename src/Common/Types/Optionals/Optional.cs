using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Application;

public readonly struct Optional : IMaybe
{
    public bool HasValue   => !HasNoValue;
    public bool HasNoValue => true;

    public static Optional None => new();

    /// <summary>
    ///     Creates a new <see cref="Optional{T}" /> from the provided <paramref name="value" />
    /// </summary>
    public static Optional<T> From<T>(T value) =>
        Optional<T>.From(value);
}

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly struct Optional<T> : IEquatable<Optional<T>>, IEquatable<object>, IOptional<T>
{
    private readonly T _value;

    public T Value => GetValueOrThrow();

    public static Optional<T> None => new();

    public bool HasValue { get; }

    public bool HasNoValue => !HasValue;

    private Optional(T value)
    {
        if (value == null)
        {
            HasValue = false;
            _value   = default!;
            return;
        }

        HasValue = true;
        _value   = value;
    }

    public T GetValueOrThrow() =>
        GetValueOrThrow(Configuration.NoValueException);

    public T GetValueOrThrow(string errorMessage)
    {
        if (HasNoValue)
        {
            throw new InvalidOperationException(errorMessage);
        }

        return _value;
    }

    public T GetValueOrThrow(Exception exception)
    {
        if (HasNoValue)
        {
            throw exception;
        }

        return _value;
    }

    public T GetValueOrDefault(T defaultValue = default!) =>
        HasNoValue
            ? defaultValue
            : _value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetValue(
        [NotNullWhen(true), MaybeNullWhen(false)]
        out T value)
    {
        value = _value;
        return HasValue;
    }

    #region // Overrides

    public static implicit operator Optional<T>(T value)
    {
        if (value is Optional<T> m)
        {
            return m;
        }

        return Optional.From(value);
    }

    public static implicit operator Optional<T>(Optional value) =>
        None;

    public static Optional<T> From(T obj) =>
        new(obj);

    public static bool operator ==(Optional<T> optional, T value)
    {
        if (value is Optional<T>)
        {
            return optional.Equals(value);
        }

        if (optional.HasNoValue)
        {
            return value is null;
        }

        return optional._value!.Equals(value);
    }

    public static bool operator !=(Optional<T> optional, T value) =>
        !(optional == value!);

    public static bool operator ==(Optional<T> optional, object other) =>
        optional.Equals(other);

    public static bool operator !=(Optional<T> optional, object other) =>
        !(optional == other);

    public static bool operator ==(Optional<T> first, Optional<T> second) =>
        first.Equals(second);

    public static bool operator !=(Optional<T> first, Optional<T> second) =>
        !(first == second);

    public override bool Equals(object? obj) =>
        obj switch
        {
            null           => false,
            Optional<T> other => Equals(other),
            T value        => Equals(value),
            _              => false
        };

    public bool Equals(Optional<T> other)
    {
        if (HasNoValue && other.HasNoValue)
        {
            return true;
        }

        if (HasNoValue || other.HasNoValue)
        {
            return false;
        }

        return EqualityComparer<T>.Default.Equals(_value, other._value);
    }

    public override int GetHashCode() =>
        HasNoValue
            ? 0
            : _value?.GetHashCode() ?? default;

    public override string ToString() =>
        HasNoValue
            ? "No value"
            : _value?.ToString() ?? string.Empty;

    #endregion

    private static class Configuration
	{
        // ReSharper disable once StaticMemberInGenericType
        public static string NoValueException = "Maybe has no value";
	}
}
