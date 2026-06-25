using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Abwaab.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedDataController : ControllerBase
    {
        [HttpPost]
        public IActionResult Seed()
        {
            // Implement your seeding logic here
            // For example, you can call a service that seeds the database with initial data
            return Ok("Data seeded successfully.");
        }
    }
}
