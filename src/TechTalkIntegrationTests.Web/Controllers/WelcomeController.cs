using Microsoft.AspNetCore.Mvc;

namespace TechTalkIntegrationTests.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WelcomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Welcome");
        }
    }
}
