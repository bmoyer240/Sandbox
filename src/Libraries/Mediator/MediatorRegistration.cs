using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class MediatorRegistration
{
    public static IServiceCollection AddMediatorInternal(this IServiceCollection services, IConfiguration configuration) =>
        services.AddMediatorInternal(configuration, _ => {});

    public static IServiceCollection AddMediatorInternal(this IServiceCollection services, IConfiguration configuration, Action<MediatorOptions> options)
    {
        services
            .AddOptions<MediatorSettings>()
            .Bind(configuration.GetSection(MediatorSettings.ConfigurationKey))
            .ValidateDataAnnotations();

        services.AddMediator(options);

        return services;
    }
}


