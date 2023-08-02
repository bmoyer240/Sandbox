namespace Application;

public interface IErrorOr<out TValue> : IErrorOr
{
    /// <summary>
    ///     The value associated with the result.
    /// </summary>
    TValue Value { get; }
}

public interface IErrorOr
{
    /// <summary>
    ///     Gets the list of errors.
    /// </summary>
    IReadOnlyList<Error> Errors { get; }

    /// <summary>
    ///     Indicates whether the result represents a failure.
    /// </summary>
    bool IsError { get; }

    /// <summary>
    ///     Indicates whether the result represents a success.
    /// </summary>
    bool IsSuccess { get; }
}
