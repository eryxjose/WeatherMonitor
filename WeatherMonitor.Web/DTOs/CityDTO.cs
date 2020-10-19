using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherMonitor.Web.DTOs
{
    public class CityDTO
    {
        public int Id { get; set; }
        public int Name { get; set; }

        public virtual List<WeatherEntryDTO> WeatherEntries { get; set; }
    }

    public class CityCreateDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
