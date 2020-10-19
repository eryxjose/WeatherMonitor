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
        private static Timer aTimer;

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

        //private static void SetRequestInterval()
        //{
        //    aTimer = new System.Timers.Timer();
        //    aTimer.Interval = 26000; // 
        //    aTimer.Elapsed += OnTimedEvent;
        //    aTimer.AutoReset = true;
        //    aTimer.Enabled = true;
        //}

        //private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        //{
            
        //    //var service = new UpdateService();
        //    //Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
        //}
    }
}
