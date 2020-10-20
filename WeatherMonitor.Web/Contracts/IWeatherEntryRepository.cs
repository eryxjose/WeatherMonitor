using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherMonitor.Web.Models;

namespace WeatherMonitor.Web.Contracts
{
    public interface IWeatherEntryRepository : IRepositoryBase<WeatherEntry>
    {
        Task<IList<WeatherEntry>> FindByCityAndPeriod(string city, DateTime start, DateTime end);
    }
}
