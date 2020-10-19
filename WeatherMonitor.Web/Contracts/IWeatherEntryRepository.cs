using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherMonitor.Web.Models;

namespace WeatherMonitor.Web.Contracts
{
    public interface IWeatherEntryRepository : IRepositoryBase<WeatherEntry>
    {
    }
}
