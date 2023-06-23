using Dapper;

namespace Microsoft.Extensions.DependencyInjection;

public static class Registration
{
    public static IServiceCollection AddDapperTypeHandlers(this IServiceCollection services)
    {
        DateOnlyArrayHandler.Register();
        DateOnlyTypeHandler.Register();
        DecimalArrayHandler.Register();
        IntArrayHandler.Register();
        StringArrayHandler.Register();
        TimeOnlyTypeHandler.Register();

        return services;
    }
}
