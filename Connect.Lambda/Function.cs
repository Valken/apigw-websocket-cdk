using Amazon.ApiGatewayManagementApi;
using Amazon.ApiGatewayManagementApi.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using AWS.Lambda.Powertools.Logging;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Connect.Lambda;

public class Function
{
    private static AmazonApiGatewayManagementApiClient _apiGatewayManagementApiClient = new(
        new AmazonApiGatewayManagementApiConfig()
        {
            ServiceURL = Environment.GetEnvironmentVariable("WEBSOCKET_URL")
        });
    
    [Logging]
    public async Task<APIGatewayProxyResponse> Handler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        if (request.RequestContext.EventType == "MESSAGE")
        {
            Logger.LogInformation(new { request.RequestContext.ConnectionId, request.RequestContext.EventType }, "Received message from client: {0}", request.Body);
            
            _ = await _apiGatewayManagementApiClient.PostToConnectionAsync(new PostToConnectionRequest
            {
                ConnectionId = request.RequestContext.ConnectionId,
                Data = new MemoryStream("Hello from the server!"u8.ToArray())
            });
        }
        
        return await Task.FromResult(new APIGatewayProxyResponse { StatusCode = 200 });
    }
}