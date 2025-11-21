using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ComandesAPI.DTOs;

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
        /// <returns>Informació de l'estat del servei, versió i data de compilació</returns>
        /// <response code="200">Servei actiu i funcionant correctament</response>
        [HttpGet]
        [ProducesResponseType(typeof(HealthDto), StatusCodes.Status200OK)]
        public IActionResult Check()
        {
            var assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            var version = assemblyVersion != null 
                ? $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}" 
                : "1.0.0";

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var build = new System.IO.FileInfo(assembly.Location).LastWriteTime.ToString("yyyyMMddHHmm");

            return Ok(new HealthDto
            {
                Status = "Servei actiu",
                Timestamp = DateTime.UtcNow,
                Service = "API de Comandes JDSR",
                Version = version,
                Build = build
            });
        }
    }
}
