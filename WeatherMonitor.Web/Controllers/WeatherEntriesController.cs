using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherMonitor.Web.Contracts;
using WeatherMonitor.Web.DTOs;
using WeatherMonitor.Web.Models;

namespace WeatherMonitor.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherEntriesController : ControllerBase
    {

        private readonly IWeatherEntryRepository _repo;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public WeatherEntriesController(IWeatherEntryRepository repo, 
            ILoggerService logger, IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }


        /// <summary>
        /// Get all Weather Entries
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWeatherEntries()
        {
            try
            {
                var weatherEntries = await _repo.FindAll();
                var response = _mapper.Map<IList<WeatherEntryDTO>>(weatherEntries);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Get WeatherEntry by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Weather record.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWeatherEntryById(int id)
        {
            try
            {
                var weatherEntry = await _repo.FindById(id);
                if (weatherEntry == null) return NotFound();

                var response = _mapper.Map<WeatherEntryDTO>(weatherEntry);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message} - {ex.InnerException}");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] WeatherEntryCreateDTO entity)
        {
            try
            {
                if (entity == null)
                {
                    _logger.LogWarn($"Invalid attempted to create a WeatherEntry with an empty data.");
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"Invalid attempted to create a Weather Entry. More details: {System.Environment.NewLine} {entity}");
                    return BadRequest(ModelState);
                }

                var weatherEntry = _mapper.Map<WeatherEntry>(entity);
                var isSuccess = await _repo.Create(weatherEntry);

                if (!isSuccess)
                {
                    return InternalError($"Something wrong occurred. The City was not created.");
                }

                return Created("Create", new { weatherEntry });

            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message} - {ex.InnerException}");
            }
        }

        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Ocorreu um erro.");
        }

    }
}
