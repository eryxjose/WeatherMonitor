using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherMonitor.Web.Contracts;
using WeatherMonitor.Web.Controllers;
using WeatherMonitor.Web.Models;
using WeatherMonitor.Web.Services;

namespace WeatherMonitor.UnitTests.Services
{
    [TestClass]
    public class WeatherEntryRepositoryTests
    {
        
        public readonly IWeatherEntryRepository MockWeatherEntryRepository;

        public WeatherEntryRepositoryTests()
        {
            List<City> cities = new List<City>
            {
                 new City { Id = 1, Name = "Florianopolis" },
                 new City { Id = 2, Name = "Sao Paulo"},
                 new City { Id = 3, Name = "Rio de Janeiro" }
            };

            IList<WeatherEntry> entries = new List<WeatherEntry>
            {
                new WeatherEntry { City = cities.Find(o => o.Id == 3), CityId = 3, Id = 1, WeatherJson = "aaaa"},
                new WeatherEntry { City = cities.Find(o => o.Id == 2), CityId = 2, Id = 2, WeatherJson = "bbbb"},
                new WeatherEntry { City = cities.Find(o => o.Id == 1), CityId = 1, Id = 3, WeatherJson = "cccc"},
                new WeatherEntry { City = cities.Find(o => o.Id == 3), CityId = 3, Id = 2, WeatherJson = "ddd"}
            };

            Mock<IWeatherEntryRepository> mockRepository = new Mock<IWeatherEntryRepository>();

            mockRepository.Setup(o => o.FindAll()).ReturnsAsync(entries);
            mockRepository.Setup(o => o.FindByCityAndPeriod(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync((string s, DateTime d1, DateTime d2) => entries.Where(x => x.City.Name == s).ToList());

            this.MockWeatherEntryRepository = mockRepository.Object;
        }

        [TestMethod]
        public async void FindByCityAndPeriod_FindAll__ReturnAll()
        {
            var result = await this.MockWeatherEntryRepository.FindAll();

            Assert.AreEqual(result.Count, 4);
        }

        [TestMethod]
        public async void FindByCityAndPeriod_WithProperArguments_ReturnWeatherEntries()
        {
            string city = "Florianopolis";
            DateTime startDate = DateTime.Now.AddDays(-1);
            DateTime endDate = DateTime.Now;

            var result = await this.MockWeatherEntryRepository.FindByCityAndPeriod(city, startDate, endDate);

            Assert.AreEqual(result.Count, 1);
        }

    }
}
