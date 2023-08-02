namespace Application;

/// <summary>
///     A discriminated union of errors or a value.
/// </summary>
public readonly record struct ErrorOr<TValue> : IErrorOr<TValue>
{
    private readonly TValue?           _value  = default;
    private readonly Lazy<List<Error>> _errors = new();

    private static readonly Error NoFirstError = Error.Unexpected(
        "ErrorOr.NoFirstError",
        "First error cannot be retrieved from a successful ErrorOr.");

    private static readonly Error NoErrors = Error.Unexpected(
        "ErrorOr.NoErrors",
        "Error list cannot be retrieved from a successful ErrorOr.");

    public bool IsError { get; }
    public bool IsSuccess => !IsError;
    public TValue Value => _value!;


    public IReadOnlyList<Error> Errors =>
        IsError && _errors?.IsValueCreated == true
            ? _errors.Value.AsReadOnly()
            : Array.Empty<Error>();


    /// <summary>
    ///     Creates an <see cref="ErrorOr{TValue}" /> from a list of errors.
    /// </summary>
    public static ErrorOr<TValue> From(List<Error> errors) =>
        errors;

    public Error FirstError
    {
        get => (IsError && _errors?.IsValueCreated == true)
                ? _errors.Value[0]
                : NoFirstError;
    }

    private ErrorOr(Error error)
    {
        _errors.Value.Add(error);
        IsError = true;
    }

    private ErrorOr(IEnumerable<Error> errors)
    {
        _errors.Value.AddRange(errors);
        IsError = true;
    }

    private ErrorOr(TValue value)
    {
        _value  = value;
        IsError = false;
    }

    /// <summary>
    ///     Creates an <see cref="ErrorOr{TValue}" /> from a value.
    /// </summary>
    public static implicit operator ErrorOr<TValue>(TValue value) =>
        new(value);

    /// <summary>
    ///     Creates an <see cref="ErrorOr{TValue}" /> from an error.
    /// </summary>
    public static implicit operator ErrorOr<TValue>(Error error) =>
        new(error);

    /// <summary>
    ///     Creates an <see cref="ErrorOr{TValue}" /> from a list of errors.
    /// </summary>
    public static implicit operator ErrorOr<TValue>(List<Error> errors) =>
        new(errors);

    /// <summary>
    ///     Creates an <see cref="ErrorOr{TValue}" /> from a list of errors.
    /// </summary>
    public static implicit operator ErrorOr<TValue>(Error[] errors) =>
        new(errors.ToList());

    public void Switch(Action<TValue> onValue, Action<List<Error>> onError)
    {
        if (IsError)
        {
            onError(_errors.Value);
            return;
        }

        onValue(Value);
    }

    public async Task SwitchAsync(Func<TValue, Task> onValue, Func<List<Error>, Task> onError)
    {
        if (IsError)
        {
            await onError(_errors.Value).ConfigureAwait(false);
            return;
        }

        await onValue(Value)
            .ConfigureAwait(false);
    }

    public void SwitchFirst(Action<TValue> onValue, Action<Error> onFirstError)
    {
        if (IsError)
        {
            onFirstError(FirstError);
            return;
        }

        onValue(Value);
    }

    public async Task SwitchFirstAsync(Func<TValue, Task> onValue, Func<Error, Task> onFirstError)
    {
        if (IsError)
        {
            await onFirstError(FirstError).ConfigureAwait(false);
            return;
        }

        await onValue(Value)
            .ConfigureAwait(false);
    }

    public TResult Match<TResult>(Func<TValue, TResult> onValue, Func<List<Error>, TResult> onError) =>
        IsError
            ? onError(_errors.Value)
            : onValue(Value);

    public async Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> onValue, Func<List<Error>, Task<TResult>> onError)
    {
        if (IsError)
        {
            return await onError(_errors.Value).ConfigureAwait(false);
        }

        return await onValue(Value).ConfigureAwait(false);
    }

    public TResult MatchFirst<TResult>(Func<TValue, TResult> onValue, Func<Error, TResult> onFirstError) =>
        IsError
            ? onFirstError(FirstError)
            : onValue(Value);

    public async Task<TResult> MatchFirstAsync<TResult>(Func<TValue, Task<TResult>> onValue, Func<Error, Task<TResult>> onFirstError)
    {
        if (IsError)
        {
            return await onFirstError(FirstError)
                       .ConfigureAwait(false);
        }

        return await onValue(Value)
                   .ConfigureAwait(false);
    }
}

public static class ErrorOr
{
    public static ErrorOr<TValue> From<TValue>(TValue value) =>
        value;
}
