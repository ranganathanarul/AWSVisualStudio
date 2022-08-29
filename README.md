# AWSVisualStudio


 Dependency injection in .NET : https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection
 
            var servicecollection = new ServiceCollection();
            ConfigureServices(servicecollection);

            ////Create service provider
            var serviceProvider = servicecollection.BuildServiceProvider();

           
            //Entry from the application.
            var grade = serviceProvider.GetService<DependencyInjectionGrade>().GradeLevelcheck(empName);
            
             private static void ConfigureServices(IServiceCollection serviceCollection)
            {
            //Dependencies Injection
            serviceCollection.AddTransient<DependencyInjectionGrade>();
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
 
 API Gateway  : https://docs.aws.amazon.com/apigateway/latest/developerguide/welcome.html
 API Rest : https://docs.aws.amazon.com/apigateway/latest/developerguide/apigateway-rest-api.html
 
 
 Lambda Function : https://docs.aws.amazon.com/lambda/latest/dg/welcome.html
 
