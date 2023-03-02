namespace Mediator;

/// <summary>
///     Logs all unhandle exceptions as an error
/// </summary>
/// <remarks>
///     This handler should be registered first
/// </remarks>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <exception cref="Exception"></exception>
internal sealed class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehavior(ILogger<TRequest> logger) =>
        _logger = logger;

    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        try
        {
            return await next(message, cancellationToken);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Unhandled Exception for Request {Name} {@Request}",
                typeof(TRequest).Name,
                message);

            throw;
        }
    }
}