using Microsoft.AspNetCore.Http;

namespace MediatRpc.Tools.Scalar;

public static class ScalarProxyResults
{
    public static IResult Ok(object data) => Results.Json(new ScalarProxyResponse
    {
        Status = 200,
        Headers = new ScalarProxyHeader
        {
            ContentType = "application/json"
        },
        Data = data
    });
}
