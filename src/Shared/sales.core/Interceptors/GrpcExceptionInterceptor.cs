using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace sales.core.Interceptors;

public class GrpcExceptionInterceptor(ILogger<GrpcExceptionInterceptor> logger) : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation
    )
    {
        try
        {
            return await continuation(request, context);
        }
        catch (RpcException ex)
        {
            // This was already translated by our Extension in the Service
            logger.LogWarning(
                "Business error in gRPC {Method}: {Status}",
                context.Method,
                ex.Status.Detail
            );
            throw;
        }
        catch (Exception ex)
        {
            // This is an unhandled bug or infra failure (The "Black Swan")
            logger.LogError(ex, "Unhandled exception in gRPC {Method}", context.Method);

            // Fallback to internal error to avoid leaking stack traces
            throw new RpcException(
                new Status(StatusCode.Internal, "An internal server error occurred")
            );
        }
    }
}
