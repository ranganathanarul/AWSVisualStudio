using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWS_Credentials
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> FunctionHandler(string input, ILambdaContext context)
        {
            var configBuilder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appSettings.json")
                                    .AddUserSecrets<Function>()
                                    .AddEnvironmentVariables()
                                    .Build();


            //var accessKey = configBuilder.GetValue<string>("AccessKey");
            //var secret = configBuilder.GetValue<string>("Secret");
            var preFix = configBuilder.GetValue<string>("Prefix");


            //var credentials = new BasicAWSCredentials(accessKey, secret);
            //var clientResponse = new AmazonSQSClient(credentials, Amazon.RegionEndpoint.USEast1);
            var clientResponse = new AmazonSQSClient();
            var request = new SendMessageRequest()
            {
                QueueUrl = "https://sqs.us-east-1.amazonaws.com/554441675079/Dev-Queue",
                MessageBody = preFix + input
            };
            var reponsefromQueue =  await clientResponse.SendMessageAsync(request);
            return $"{preFix} {input.ToUpper()} - { reponsefromQueue.HttpStatusCode }";
        }
    }
}
