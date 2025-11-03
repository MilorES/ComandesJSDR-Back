using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComandesAPI.Data;
using ComandesAPI.Models;
using ComandesAPI.DTOs;
using System.Security.Claims;

namespace ComandesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requereix autenticació per a tots els endpoints
    public class ClientsController : ControllerBase
    {
        private readonly ComandesDbContext _context;
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(ComandesDbContext context, ILogger<ClientsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obté tots els clients (només administradors)
        /// </summary>
        /// <returns>Llista de clients</returns>
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetClients([FromQuery] bool? actius = null)
        {
            try
            {
                var query = _context.Clients
                    .Include(c => c.Usuari)
                    .AsQueryable();

                if (actius.HasValue)
                {
                    query = query.Where(c => c.Actiu == actius.Value);
                }

                var clients = await query
                    .OrderBy(c => c.NomEmpresa)
                    .Select(c => MapToClientDto(c))
                    .ToListAsync();

                return Ok(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir els clients");
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Obté el client de l'usuari autentificat
        /// </summary>
        /// <returns>Client de l'usuari</returns>
        [HttpGet("me")]
        public async Task<ActionResult<ClientDto>> GetMyClient()
        {
            try
            {
                var userId = GetCurrentUserId();

                var client = await _context.Clients
                    .Include(c => c.Usuari)
                    .FirstOrDefaultAsync(c => c.UsuariId == userId);

                if (client == null)
                {
                    return NotFound("L'usuari no té un client associat");
                }

                return Ok(MapToClientDto(client));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir el client de l'usuari autentificat");
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Obté un client específic per ID (només administradors)
        /// </summary>
        /// <param name="id">ID del client</param>
        /// <returns>Client sol·licitat</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ClientDto>> GetClient(int id)
        {
            try
            {
                var client = await _context.Clients
                    .Include(c => c.Usuari)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (client == null)
                {
                    return NotFound("Client no trobat");
                }

                return Ok(MapToClientDto(client));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir el client {Id}", id);
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Crea un client per l'usuari autentificat
        /// </summary>
        /// <param name="createDto">Dades del nou client</param>
        /// <returns>Client creat</returns>
        [HttpPost]
        public async Task<ActionResult<ClientDto>> CreateClient([FromBody] CreateClientDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                // Verificar que l'usuari no tingui ja un client
                var existingClient = await _context.Clients
                    .FirstOrDefaultAsync(c => c.UsuariId == userId);

                if (existingClient != null)
                {
                    return BadRequest("L'usuari ja té un client associat");
                }

                // Verificar que el NIF no existeixi
                var nifExists = await _context.Clients
                    .AnyAsync(c => c.NIF == createDto.NIF);

                if (nifExists)
                {
                    return BadRequest("Ja existeix un client amb aquest NIF");
                }

                var client = new Client
                {
                    UsuariId = userId,
                    NomEmpresa = createDto.NomEmpresa,
                    NIF = createDto.NIF,
                    Adreca = createDto.Adreca,
                    Poblacio = createDto.Poblacio,
                    Provincia = createDto.Provincia,
                    CodiPostal = createDto.CodiPostal,
                    Pais = createDto.Pais,
                    Telefon = createDto.Telefon,
                    Notes = createDto.Notes,
                    DataCreacio = DateTime.UtcNow,
                    Actiu = true
                };

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                // Recarregar amb includes
                client = await _context.Clients
                    .Include(c => c.Usuari)
                    .FirstOrDefaultAsync(c => c.Id == client.Id);

                return CreatedAtAction(nameof(GetClient), new { id = client!.Id }, MapToClientDto(client));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el client");
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Actualitza el client de l'usuari autentificat
        /// </summary>
        /// <param name="updateDto">Dades actualitzades</param>
        /// <returns>Client actualitzat</returns>
        [HttpPut("me")]
        public async Task<ActionResult<ClientDto>> UpdateMyClient([FromBody] UpdateClientDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                var client = await _context.Clients
                    .FirstOrDefaultAsync(c => c.UsuariId == userId);

                if (client == null)
                {
                    return NotFound("L'usuari no té un client associat");
                }

                // Verificar que el NIF no existeixi en un altre client
                var nifExists = await _context.Clients
                    .AnyAsync(c => c.NIF == updateDto.NIF && c.Id != client.Id);

                if (nifExists)
                {
                    return BadRequest("Ja existeix un altre client amb aquest NIF");
                }

                // Actualitzar propietats
                client.NomEmpresa = updateDto.NomEmpresa;
                client.NIF = updateDto.NIF;
                client.Adreca = updateDto.Adreca;
                client.Poblacio = updateDto.Poblacio;
                client.Provincia = updateDto.Provincia;
                client.CodiPostal = updateDto.CodiPostal;
                client.Pais = updateDto.Pais;
                client.Telefon = updateDto.Telefon;
                client.Notes = updateDto.Notes;
                client.Actiu = updateDto.Actiu;
                client.DataModificacio = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Recarregar amb includes
                client = await _context.Clients
                    .Include(c => c.Usuari)
                    .FirstOrDefaultAsync(c => c.Id == client.Id);

                return Ok(MapToClientDto(client!));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualitzar el client");
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Actualitza un client específic (només administradors)
        /// </summary>
        /// <param name="id">ID del client</param>
        /// <param name="updateDto">Dades actualitzades</param>
        /// <returns>Client actualitzat</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ClientDto>> UpdateClient(int id, [FromBody] UpdateClientDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var client = await _context.Clients.FindAsync(id);

                if (client == null)
                {
                    return NotFound("Client no trobat");
                }

                // Verificar que el NIF no existeixi en un altre client
                var nifExists = await _context.Clients
                    .AnyAsync(c => c.NIF == updateDto.NIF && c.Id != id);

                if (nifExists)
                {
                    return BadRequest("Ja existeix un altre client amb aquest NIF");
                }

                // Actualitzar propietats
                client.NomEmpresa = updateDto.NomEmpresa;
                client.NIF = updateDto.NIF;
                client.Adreca = updateDto.Adreca;
                client.Poblacio = updateDto.Poblacio;
                client.Provincia = updateDto.Provincia;
                client.CodiPostal = updateDto.CodiPostal;
                client.Pais = updateDto.Pais;
                client.Telefon = updateDto.Telefon;
                client.Notes = updateDto.Notes;
                client.Actiu = updateDto.Actiu;
                client.DataModificacio = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Recarregar amb includes
                client = await _context.Clients
                    .Include(c => c.Usuari)
                    .FirstOrDefaultAsync(c => c.Id == id);

                return Ok(MapToClientDto(client!));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualitzar el client {Id}", id);
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Elimina (desactiva) un client (només administradors)
        /// </summary>
        /// <param name="id">ID del client</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            try
            {
                var client = await _context.Clients.FindAsync(id);

                if (client == null)
                {
                    return NotFound("Client no trobat");
                }

                // Desactivar en lloc d'eliminar
                client.Actiu = false;
                client.DataModificacio = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el client {Id}", id);
                return StatusCode(500, "Error intern del servidor");
            }
        }

        // Mètodes auxiliars privats

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim ?? "0");
        }

        private static ClientDto MapToClientDto(Client client)
        {
            return new ClientDto
            {
                Id = client.Id,
                UsuariId = client.UsuariId,
                NomEmpresa = client.NomEmpresa,
                NIF = client.NIF,
                Adreca = client.Adreca,
                Poblacio = client.Poblacio,
                Provincia = client.Provincia,
                CodiPostal = client.CodiPostal,
                Pais = client.Pais,
                Telefon = client.Telefon,
                Notes = client.Notes,
                Actiu = client.Actiu,
                DataCreacio = client.DataCreacio,
                DataModificacio = client.DataModificacio
            };
        }
    }
}
