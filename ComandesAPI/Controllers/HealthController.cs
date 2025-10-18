using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComandesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous] // Endpoint públic
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Endpoint públic per verificar que el servei està actiu
        /// </summary>
        [HttpGet]
        public IActionResult Check()
        {
            return Ok(new
            {
                status = "Servei actiu",
                timestamp = DateTime.UtcNow,
                service = "API de Comandes JDSR",
                version = "1.0.0"
            });
        }
    }
}
