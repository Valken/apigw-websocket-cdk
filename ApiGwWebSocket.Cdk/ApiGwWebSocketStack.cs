using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.Apigatewayv2;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AwsApigatewayv2Integrations;
using Constructs;

namespace ApiGwWebSocket
{
    public class ApiGwWebSocketStack : Stack
    {
        internal ApiGwWebSocketStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            // The code that defines your stack goes here
            var lambdaFunction = new Function(this, "MyFirstLambda", new FunctionProps
            {
                Runtime = Runtime.DOTNET_8,
                Handler = "Connect.Lambda::Connect.Lambda.Function::Handler",
                Code = Code.FromAsset("Connect.Lambda/bin/Debug/net8.0"),
                MemorySize = 256,
                Timeout = Duration.Seconds(30),
                Environment = new Dictionary<string, string>()
                {
                    ["POWERTOOLS_SERVICE_NAME"] = "websocket-connect-handler",
                    ["POWERTOOLS_LOG_LEVEL"] = "Debug",
                    ["POWERTOOLS_LOGGER_LOG_EVENT"] = "true"
                }
            });
            
            var webSocketApi = new WebSocketApi(this, "webSocketAPi", new WebSocketApiProps
                {
                Description = "My WebSocket API",
                ConnectRouteOptions = new WebSocketRouteOptions
                {
                    Integration = new WebSocketLambdaIntegration("connect", lambdaFunction)
                },
            });
            
            var stage = new WebSocketStage(this, "stage", new WebSocketStageProps
            {
                StageName = "dev",
                WebSocketApi = webSocketApi,
                AutoDeploy = true
            });
        }
    }
}
