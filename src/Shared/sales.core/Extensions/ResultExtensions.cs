using Grpc.Core;
using sales.Contracts.Responses;

namespace sales.core.Extensions;

public static class ResultExtensions
{
    public static StatusCode ToGrpcStatusCode(this int statusCode) =>
        statusCode switch
        {
            200 or 201 or 204 => StatusCode.OK,
            400 => StatusCode.InvalidArgument,
            401 => StatusCode.Unauthenticated,
            403 => StatusCode.PermissionDenied,
            404 => StatusCode.NotFound,
            409 => StatusCode.AlreadyExists,
            422 => StatusCode.FailedPrecondition,
            503 => StatusCode.Unavailable,
            _ => StatusCode.Internal,
        };

    public static RpcException ToRpcException<T>(this TResponse<T> response)
    {
        var status = new Status(
            response.StatusCode.ToGrpcStatusCode(),
            response.Message ?? "Erro desconhecido"
        );
        return new RpcException(status);
    }
}
