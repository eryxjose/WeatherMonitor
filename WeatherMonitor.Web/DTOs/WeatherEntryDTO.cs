using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherMonitor.Web.DTOs
{
    public class WeatherEntryDTO
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string WeatherJson { get; set; }

    }

    public class WeatherEntryCreateDTO
    {
        [Required]
        public int CityId { get; set; }
        
        [Required]
        public string WeatherJson { get; set; }
    }

}
