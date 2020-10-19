using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherMonitor.Web.DTOs;
using WeatherMonitor.Web.Models;

namespace WeatherMonitor.Web.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<City, CityDTO>().ReverseMap();
            CreateMap<WeatherEntry, WeatherEntryDTO>().ReverseMap();

        }
    }
}
