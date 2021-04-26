using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using WorldGeneration;
using Player;

namespace ASD_project
{
    partial class Program
    {
        //Setup logger and injection
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Application starting");

            //Example of dependency injection with GreetingService
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IMainGame, MainGame>();
                    services.AddScoped<IPlayerModel, PlayerModel>();
                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<MainGame>(host.Services);
            svc.Run();
        }

        static void BuildConfig(IConfigurationBuilder builder) 
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();

        }
    }
}
