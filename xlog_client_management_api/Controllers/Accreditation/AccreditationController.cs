using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xlog_client_management_api.Controllers.Accreditation
{
    [ApiExplorerSettings(GroupName = "v2")]
    [Route("clients/api/v1")]

    [ApiController]
    public class AccreditationController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
       {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IAuthorizationService _authorizationService;

        public AccreditationController(
            ILogger<WeatherForecastController> logger,
            IAuthorizationService authorizationService
            )
        {
            _logger = logger;
            _authorizationService = authorizationService;
        }
        [HttpGet]
        [Route("time-info")]
        public IActionResult GetTimeInfo()
        {
            return Ok(TimeZoneInfo.GetSystemTimeZones());
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
