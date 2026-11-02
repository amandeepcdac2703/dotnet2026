using Asp.Versioning;
using dotnet2026.Infrastructure.External;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotnet2026.Api.Controllers
{
    [ApiController]
    //[Route("api/v{version:apiVersion}/[controller]")]
    //[ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class UniversitiesController : ControllerBase
    {
        private readonly IHttpClientFactory _factory;
        private readonly UniversityService _univeresityService;

        public UniversitiesController(IHttpClientFactory factory, UniversityService univeresityService)
        {
            this._factory = factory;
            this._univeresityService = univeresityService;
        }

        // direct call
        [HttpGet("v1/{country}")]
        public async Task<IActionResult> GetList([FromRoute] string country)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://universities.hipolabs.com/");

            var response = await client.GetAsync($"search?country={country}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                
                return Ok(result);
            }
            return BadRequest();
        }

         
        // regist the builder.Services.AddHttpClient() in program.cs file //  httpfactory without named client
        [HttpGet("v2/{country}")]
        public async Task<IActionResult> GetListV2([FromRoute] string country)
        {
            HttpClient _client = _factory.CreateClient();
            _client.BaseAddress = new Uri("http://universities.hipolabs.com/");

            var response = await _client.GetAsync($"search?country={country}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                return Ok(result);
            }
            return BadRequest();
        }


        // V3: Using Named HttpClient configured in Program.cs // Named client
        [HttpGet("v3/{country}")]
        public async Task<IActionResult> GetListV3(
            [FromRoute] string country)
        {
            HttpClient client = _factory.CreateClient("UniversityApi");

            var response = await client.GetAsync(
                $"search?country={Uri.EscapeDataString(country)}");

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();

            return Content(result, "application/json");
        }

        [HttpGet("v4/{country}")]
        public async Task<IActionResult> GetListV4(
        [FromRoute] string country,
        CancellationToken cancellationToken)
        {
            var result = await _univeresityService
                .GetUniversitiesByCountry(country, cancellationToken);

            return Ok(result);
        }
    }
}
