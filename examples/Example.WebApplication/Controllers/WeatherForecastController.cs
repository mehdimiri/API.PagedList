using API.PagedList;
using API.PagedList.Model;
using Microsoft.AspNetCore.Mvc;

namespace Example.WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(ILogger<WeatherForecastController> logger) : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        private readonly ILogger<WeatherForecastController> _logger = logger;

        [HttpPost("GetWeatherForecast")]
        public dynamic Get(FilterVM filter)
        {
            IQueryable<WeatherForecast> models = Enumerable.Range(1, 10).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).AsQueryable();
           return models.ToPagedList(filter);
        }
    }
}
