using Microsoft.Extensions.DependencyInjection;
using sales.core.Interceptors;

namespace sales.core.Extensions;

public static class GrpcExtensions
{
    public static void AddCustomGrpc(this IServiceCollection services)
    {
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<GrpcExceptionInterceptor>();
        });
    }
}
