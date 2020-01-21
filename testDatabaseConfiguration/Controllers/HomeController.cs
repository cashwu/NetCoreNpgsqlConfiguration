using System.Linq;
using System.Threading.Tasks;
using Cashwu.AspNetCore.Configuration.PostgreSQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace testDatabaseConfiguration.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ConfigurationContext _configurationContext;

        public HomeController(IConfiguration configuration, ConfigurationContext configurationContext)
        {
            _configuration = configuration;
            _configurationContext = configurationContext;
        }

        [Route("/")]
        [HttpGet]
        public IActionResult Index()
        {
            var dictionary = _configuration.AsEnumerable()
                                           .Where(a => a.Key.StartsWith("C_"))
                                           .ToDictionary(a => a.Key, b => b.Value);

            return Ok(dictionary);
        }

        [Route("/dbconfig")]
        [HttpGet]
        public async Task<IActionResult> DbConfig([FromQuery] string k, [FromQuery] string v)
        {
            if (!string.IsNullOrWhiteSpace(k)
                && !string.IsNullOrWhiteSpace(v))
            {
                var configurationValue = await _configurationContext.ConfigurationValue.FirstOrDefaultAsync(a => a.Key == k);

                if (configurationValue != null)
                {
                    configurationValue.Value = v;
                    await _configurationContext.SaveChangesAsync();
                }
            }

            var data = await _configurationContext.ConfigurationValue.ToListAsync();

            return Ok(data);
        }
    }
}