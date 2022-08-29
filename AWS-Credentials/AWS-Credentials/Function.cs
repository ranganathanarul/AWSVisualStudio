using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWS_Credentials
{


    public class Function
    {
        private static readonly string[] Employee = new[] {
            "Ranganathan" , "Palanisamy", "Ravi","Raja","Alex","Arul","Anand"
            };

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<EmployeeInfo> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            //var configBuilder = new ConfigurationBuilder()
            //                        .SetBasePath(Directory.GetCurrentDirectory())
            //                        .AddJsonFile("appSettings.json")
            //                        .AddUserSecrets<Function>()
            //                        .AddEnvironmentVariables()
            //                        .Build();


            ////var accessKey = configBuilder.GetValue<string>("AccessKey");
            ////var secret = configBuilder.GetValue<string>("Secret");
            //var preFix = configBuilder.GetValue<string>("Prefix");


            ////var credentials = new BasicAWSCredentials(accessKey, secret);
            ////var clientResponse = new AmazonSQSClient(credentials, Amazon.RegionEndpoint.USEast1);
            //var clientResponse = new AmazonSQSClient();
            //var request = new SendMessageRequest()
            //{
            //    QueueUrl = "https://sqs.us-east-1.amazonaws.com/554441675079/Dev-Queue",
            //    MessageBody = preFix + input
            //};
            //var reponsefromQueue =  await clientResponse.SendMessageAsync(request);
            //return $"{preFix} {input.ToUpper()} - { reponsefromQueue.HttpStatusCode }";

            var servicecollection = new ServiceCollection();
            ConfigureServices(servicecollection);

            ////Create service provider
            var serviceProvider = servicecollection.BuildServiceProvider();

            Console.WriteLine(JsonSerializer.Serialize(input));

            string empName = null;
            input.QueryStringParameters?.TryGetValue("EmployeeName", out empName);
            empName = empName ?? "Ranganathan";

            //Entry from the application.
            var grade = serviceProvider.GetService<DependencyInjectionGrade>().GradeLevelcheck(empName);

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new EmployeeInfo
            {
                empGrade = grade,
                EmpName = empName,
                EmployeeId = rng.Next(),
                Firstname = "Ranganathan " + rng.Next(),
                Lastname = "Palanisamy " + rng.Next(),
                address = rng.Next() + "-XXXXX,XXXX,CT-06800"


            }).ToList();
            //return input.Body?.ToUpper();
        }

        public APIGatewayProxyResponse FunctionHandlerPost(APIGatewayProxyRequest input, ILambdaContext context)
        {
            var data = JsonSerializer.Deserialize<EmployeeInfo>(input.Body);
            string empName = "Ranganathan";
            input.PathParameters?.TryGetValue("empName", out empName);
            data.EmpName = empName;
            return new APIGatewayProxyResponse()
            {
                Body = JsonSerializer.Serialize(data),
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            //Dependencies Injection
            serviceCollection.AddTransient<DependencyInjectionGrade>();
        }
    }
    public class EmployeeInfo
    {
        public string EmpName { get; set; }

        public int EmployeeId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string address { get; set; }

        public int empGrade { get; set; }
    }
    public class DependencyInjectionGrade
    {
        public int GradeLevel { get; set; }

        public int GradeLevelcheck(string empName)
        {
            if(empName.Contains("Ranga"))
            {
                GradeLevel = 0;
            }
            else
            {
                GradeLevel=1;
            }
            return GradeLevel;
        }

    }
}
