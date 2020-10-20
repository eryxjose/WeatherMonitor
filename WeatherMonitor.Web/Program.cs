using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using WeatherMonitor.Web.Contracts;
using WeatherMonitor.Web.Data;
using WeatherMonitor.Web.Models;
using WeatherMonitor.Web.Services;

namespace WeatherMonitor.Web
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    WorkerOptions options = configuration.GetSection("ExternalApi").Get<WorkerOptions>();
                    services.AddSingleton(options);

                    services.AddHostedService<UpdateService>();
                    
                });

    }
}
