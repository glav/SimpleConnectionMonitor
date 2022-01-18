using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Connectionmonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Connection Monitor");

            var appConfig = LoadAppSettings();

            if (appConfig == null)
            {
                Console.WriteLine("Missing or invalid appsettings.json...exiting");
                return;
            }

            var endpointToMonitor = appConfig["monitorEndpoint"];
            int intervalValueInSeconds;
            if (!int.TryParse(appConfig["checkIntervalInSeconds"],out intervalValueInSeconds))
            {
                intervalValueInSeconds = 5;
            }


            Console.WriteLine("Monitoring endpoint: [{0}]",endpointToMonitor);
            Console.WriteLine("Check interval: {0} seconds",intervalValueInSeconds);

            Console.WriteLine("Starting");
            
            var connMonitor = new Monitor(new AuditLogger(),EndpointTester.CreateEndpointTester(endpointToMonitor), intervalValueInSeconds);
            connMonitor.StartMonitoring();
            Console.WriteLine("Started\n");

            Console.WriteLine("<-- Press ENTER to terminate -->");
            Console.ReadLine();

        }
        static IConfigurationRoot LoadAppSettings()
        {
			var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var appConfig = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            return appConfig;
        }
    }

}
