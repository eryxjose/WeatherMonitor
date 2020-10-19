using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherMonitor.Web.Models
{
    public class WeatherEntry
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string WeatherJson { get; set; }

        public virtual City City { get; set; }
    }
}
