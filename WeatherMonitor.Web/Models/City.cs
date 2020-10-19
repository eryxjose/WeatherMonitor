﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherMonitor.Web.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<WeatherEntry> WeatherEntries { get; set; }
    }
}