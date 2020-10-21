using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WeatherMonitor.Web.Data;
using WeatherMonitor.Web.Contracts;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using WeatherMonitor.Web.Models;
using WeatherMonitor.Web.Controllers;

namespace WeatherMonitor.Web.Services
{
    public class UpdateService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILoggerService _logger;
        private readonly IConfiguration _configuration;
        private readonly WorkerOptions _options;

        public UpdateService(IServiceScopeFactory serviceScopeFactory, WorkerOptions options)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _options = options;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                
                while (!cancellationToken.IsCancellationRequested)
                {
                    foreach (var city in GetCities(dbContext))
                    {
                        string jsonResult = LoadApiData($"{_options.ExternalApiUrl}?q={city.Name}&appid={_options.ExternalApiKey}");
                        WeatherEntry weatherEntry = new WeatherEntry
                        {
                            CityId = city.Id,
                            WeatherJson = jsonResult,
                            CreatedDate = DateTime.Now
                        };
                        dbContext.WeatherEntries.Add(weatherEntry);
                    }
                    dbContext.SaveChanges();
                    await Task.Delay(10 * 60 * 1000, cancellationToken);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        
        private List<City> GetCities(ApplicationDbContext db)
        {
            List<City> cities = new List<City>();
            var cityFlorianopolis = db.Cities.FirstOrDefault(o => o.Name.Equals("Florianopolis"));
            if (cityFlorianopolis == null)
            {
                cityFlorianopolis = new City { Name = "Florianopolis" };
                db.Cities.Add(cityFlorianopolis);
                db.SaveChanges();
            }
            cities.Add(cityFlorianopolis);

            var citySaoPaulo = db.Cities.FirstOrDefault(o => o.Name.Equals("Sao Paulo"));
            if (citySaoPaulo == null)
            {
                citySaoPaulo = new City { Name = "Sao Paulo" };
                db.Cities.Add(citySaoPaulo);
                db.SaveChanges();
            }
            cities.Add(citySaoPaulo);

            var cityRioDeJaneiro = db.Cities.FirstOrDefault(o => o.Name.Equals("Rio de Janeiro"));
            if (cityRioDeJaneiro == null)
            {
                cityRioDeJaneiro = new City { Name = "Rio de Janeiro" };
                db.Cities.Add(cityRioDeJaneiro);
                db.SaveChanges();
            }
            cities.Add(cityRioDeJaneiro);

            return cities;
        }

        private string LoadApiData(string uri)
        {
            WebResponse response = null;
            StreamReader reader = null;
            string result = String.Empty;

            if (String.IsNullOrEmpty(uri)) return null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp(new Uri(uri));
                request.Headers.Clear();
                request.ContentType = "application/json;charset=\"utf-8\"";
                request.Accept = "application/json";
                request.Method = "GET";

                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                result = reader.ReadToEnd().ToLower();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {ex.InnerException}");
            }
            finally
            {
                reader.Close();
                response.Close();
            }

            return result;
        }
    }
}
