using System.Diagnostics;

namespace Mediator;

/// <summary>
///     Tracks the execution time of each request and logs
///     an event if time exceeds the specified threshold.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
internal sealed class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;
    private readonly MediatorSettings _settings;

    public PerformanceBehavior(IOptions<MediatorSettings> options, ILogger<TRequest> logger)
    {
        _logger   = logger;
        _settings = options.Value;
    }

    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        var startTime = Stopwatch.GetTimestamp();
        var response  = await next(message, cancellationToken);
        var elapsedMs = Stopwatch.GetElapsedTime(startTime).Milliseconds;

        // FEAT: Create a configuration for PerformanceBehavior or something that all middleware can extend
        if(elapsedMs > _settings.PerformanceThresholdMs)
        {
            _logger.LogWarning("Request runtime exceeded threshold ({Threshold}) \r\n '{Name}' '{ElapsedMs}ms' {@Request}",
                _settings.PerformanceThresholdMs,
                typeof(TRequest).Name,
                elapsedMs,
                message);
        }

        return response;
    }
}

public partial class MediatorSettings
{
    /// <summary>
    ///     Specifies the threshold, in milliseconds, that will trigger a
    ///     log event if the specified threshold is exceeded.
    /// </summary>
    /// <remarks>
    ///     Default: <c>500</c>
    /// </remarks>
    public int PerformanceThresholdMs { get; init; } = 500;
}