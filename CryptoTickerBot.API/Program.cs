using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CryptoTickerBot.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     var env = hostingContext.HostingEnvironment;
                     config.SetBasePath(env.ContentRootPath);

                     config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                     config.AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                                              optional: true, reloadOnChange: true);

                     config.AddEnvironmentVariables();

                 if (args != null)
                     {
                         config.AddCommandLine(args);
                     }
                 }).ConfigureLogging(builder =>
                 {
                     builder.SetMinimumLevel(LogLevel.Information);
                     builder.AddLog4Net("log4net.config");
                 })

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
}