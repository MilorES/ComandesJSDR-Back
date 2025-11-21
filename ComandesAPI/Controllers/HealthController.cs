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
            var assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            var version = assemblyVersion != null 
                ? $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}" 
                : "1.0.0";

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var build = new System.IO.FileInfo(assembly.Location).LastWriteTime.ToString("yyyyMMddHHmm");

            return Ok(new
            {
                status = "Servei actiu",
                timestamp = DateTime.UtcNow,
                service = "API de Comandes JDSR",
                version = version,
                build = build
            });
        }
    }
}
