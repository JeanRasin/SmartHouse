using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SmartHouseWeb.Models;

namespace SmartHouseWeb.Controllers
{
    [Authorize]
    public class IntelController : Controller
    {
        private readonly ApplicationSettings _settings;

        public IntelController(IOptions<ApplicationSettings> settings)
        {
            _settings = settings.Value;
        }

        public IActionResult Introduction() => View(_settings);

        public IActionResult AspNetCore() => View(_settings);

        public IActionResult Privacy() => View(_settings);

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
