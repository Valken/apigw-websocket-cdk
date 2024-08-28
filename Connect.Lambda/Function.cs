using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using AWS.Lambda.Powertools.Logging;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Connect.Lambda;

public class Function
{
    [Logging]
    public async Task<APIGatewayProxyResponse> Handler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        context.Logger.LogLine(request.RequestContext.ConnectionId);
        
        return await Task.FromResult(new APIGatewayProxyResponse { StatusCode = 200 });
    }
}