namespace Mediator;

/// <summary>
///     All incoming requests that implement <see cref="IBaseCommand" /> and have
///     an associated validator will be validated
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <exception cref="ValidationException"></exception>
internal sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IBaseCommand
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) =>
        _validators = validators;

    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        if (!_validators.Any())
        {
            return await next(message, cancellationToken);
        }

        var context = new ValidationContext<TRequest>(message);
        var results = await Task.WhenAll(_validators.Select(x => x.ValidateAsync(context, cancellationToken)));
        var errors  = results.SelectMany(x => x.Errors).Where(x => x is not null).ToArray();

        return errors.Length == 0
                   ? await next(message, cancellationToken)
                   : throw new ValidationException(errors);
    }
}
