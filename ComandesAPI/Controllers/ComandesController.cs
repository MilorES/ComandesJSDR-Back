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
    public class ComandesController : ControllerBase
    {
        private readonly ComandesDbContext _context;
        private readonly ILogger<ComandesController> _logger;

        public ComandesController(ComandesDbContext context, ILogger<ComandesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obté totes les comandes (usuaris normals veuen només les seves, administradors veuen totes)
        /// </summary>
        /// <param name="estat">Filtre opcional per estat</param>
        /// <param name="actives">Filtre opcional per comandes actives</param>
        /// <returns>Llista de comandes</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComandaDto>>> GetComandes(
            [FromQuery] EstatComanda? estat = null,
            [FromQuery] bool? actives = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                var isAdmin = User.IsInRole("Administrator");

                var query = _context.Comandes
                    .Include(c => c.Usuari)
                    .Include(c => c.Linies)
                        .ThenInclude(l => l.Article)
                    .AsQueryable();

                // Si no és administrador, només pot veure les seves comandes
                if (!isAdmin)
                {
                    query = query.Where(c => c.UsuariId == userId);
                }

                if (estat.HasValue)
                {
                    query = query.Where(c => c.Estat == estat.Value);
                }

                if (actives.HasValue)
                {
                    query = query.Where(c => c.Actiu == actives.Value);
                }

                var comandes = await query
                    .OrderByDescending(c => c.DataCreacio)
                    .Select(c => MapToComandaDto(c))
                    .ToListAsync();

                return Ok(comandes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir les comandes");
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Obté una comanda específica per ID
        /// </summary>
        /// <param name="id">ID de la comanda</param>
        /// <returns>Comanda sol·licitada</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ComandaDto>> GetComanda(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var isAdmin = User.IsInRole("Administrator");

                var comanda = await _context.Comandes
                    .Include(c => c.Usuari)
                    .Include(c => c.Linies)
                        .ThenInclude(l => l.Article)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (comanda == null)
                {
                    return NotFound("Comanda no trobada");
                }

                // Verificar que l'usuari pugui accedir a aquesta comanda
                if (!isAdmin && comanda.UsuariId != userId)
                {
                    return Forbid();
                }

                return Ok(MapToComandaDto(comanda));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir la comanda {Id}", id);
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Crea una nova comanda per l'usuari autentificat
        /// </summary>
        /// <param name="createDto">Dades de la nova comanda</param>
        /// <returns>Comanda creada</returns>
        [HttpPost]
        public async Task<ActionResult<ComandaDto>> CreateComanda([FromBody] CreateComandaDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                // Generar número de comanda únic
                var numeroComanda = await GenerarNumeroComanda();

                // Crear la comanda
                var comanda = new Comanda
                {
                    NumeroComanda = numeroComanda,
                    UsuariId = userId,
                    Estat = EstatComanda.Esborrany,
                    Observacions = createDto.Observacions,
                    DescomptePercentatge = createDto.DescomptePercentatge,
                    DataCreacio = DateTime.UtcNow,
                    Actiu = true
                };

                // Afegir línies
                int ordre = 0;
                foreach (var liniaDto in createDto.Linies)
                {
                    var linia = new LiniaComanda
                    {
                        ArticleId = liniaDto.ArticleId,
                        NomProducte = liniaDto.NomProducte,
                        Descripcio = liniaDto.Descripcio,
                        Quantitat = liniaDto.Quantitat,
                        PreuUnitari = liniaDto.PreuUnitari,
                        DescomptePercentatge = liniaDto.DescomptePercentatge,
                        Ordre = ordre++,
                        DataCreacio = DateTime.UtcNow
                    };

                    // Calcular imports
                    linia.Subtotal = linia.Quantitat * linia.PreuUnitari;
                    linia.ImportDescompte = linia.Subtotal * (linia.DescomptePercentatge / 100);
                    linia.Total = linia.Subtotal - linia.ImportDescompte;

                    comanda.Linies.Add(linia);
                }

                // Calcular totals de la comanda
                CalcularTotals(comanda);

                _context.Comandes.Add(comanda);
                await _context.SaveChangesAsync();

                // Recarregar amb includes
                comanda = await _context.Comandes
                    .Include(c => c.Usuari)
                    .Include(c => c.Linies)
                        .ThenInclude(l => l.Article)
                    .FirstOrDefaultAsync(c => c.Id == comanda.Id);

                return CreatedAtAction(nameof(GetComanda), new { id = comanda!.Id }, MapToComandaDto(comanda));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la comanda");
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Actualitza una comanda existent (només si està en estat Esborrany)
        /// </summary>
        /// <param name="id">ID de la comanda</param>
        /// <param name="updateDto">Dades actualitzades</param>
        /// <returns>Comanda actualitzada</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ComandaDto>> UpdateComanda(int id, [FromBody] UpdateComandaDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();
                var isAdmin = User.IsInRole("Administrator");

                var comanda = await _context.Comandes
                    .Include(c => c.Usuari)
                    .Include(c => c.Linies)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (comanda == null)
                {
                    return NotFound("Comanda no trobada");
                }

                // Verificar permisos
                if (!isAdmin && comanda.UsuariId != userId)
                {
                    return Forbid();
                }

                // Només es poden modificar comandes en estat Esborrany
                if (comanda.Estat != EstatComanda.Esborrany)
                {
                    return BadRequest("Només es poden modificar comandes en estat Esborrany");
                }

                // Actualitzar propietats
                comanda.Observacions = updateDto.Observacions;
                comanda.DescomptePercentatge = updateDto.DescomptePercentatge;
                comanda.DataModificacio = DateTime.UtcNow;

                // Eliminar línies antigues
                _context.LiniesComanda.RemoveRange(comanda.Linies);

                // Afegir noves línies
                comanda.Linies.Clear();
                int ordre = 0;
                foreach (var liniaDto in updateDto.Linies)
                {
                    var linia = new LiniaComanda
                    {
                        ArticleId = liniaDto.ArticleId,
                        NomProducte = liniaDto.NomProducte,
                        Descripcio = liniaDto.Descripcio,
                        Quantitat = liniaDto.Quantitat,
                        PreuUnitari = liniaDto.PreuUnitari,
                        DescomptePercentatge = liniaDto.DescomptePercentatge,
                        Ordre = ordre++,
                        DataCreacio = DateTime.UtcNow
                    };

                    // Calcular imports
                    linia.Subtotal = linia.Quantitat * linia.PreuUnitari;
                    linia.ImportDescompte = linia.Subtotal * (linia.DescomptePercentatge / 100);
                    linia.Total = linia.Subtotal - linia.ImportDescompte;

                    comanda.Linies.Add(linia);
                }

                // Recalcular totals
                CalcularTotals(comanda);

                await _context.SaveChangesAsync();

                // Recarregar amb includes
                comanda = await _context.Comandes
                    .Include(c => c.Usuari)
                    .Include(c => c.Linies)
                        .ThenInclude(l => l.Article)
                    .FirstOrDefaultAsync(c => c.Id == id);

                return Ok(MapToComandaDto(comanda!));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualitzar la comanda {Id}", id);
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Canvia l'estat d'una comanda
        /// </summary>
        /// <param name="id">ID de la comanda</param>
        /// <param name="canviarEstatDto">Nou estat</param>
        /// <returns>Comanda actualitzada</returns>
        [HttpPatch("{id}/estat")]
        public async Task<ActionResult<ComandaDto>> CanviarEstat(int id, [FromBody] CanviarEstatComandaDto canviarEstatDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();
                var isAdmin = User.IsInRole("Administrator");

                var comanda = await _context.Comandes
                    .Include(c => c.Usuari)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (comanda == null)
                {
                    return NotFound("Comanda no trobada");
                }

                // Verificar permisos
                if (!isAdmin && comanda.UsuariId != userId)
                {
                    return Forbid();
                }

                // Actualitzar estat
                comanda.Estat = canviarEstatDto.Estat;
                comanda.DataModificacio = DateTime.UtcNow;

                // Actualitzar dates segons l'estat
                switch (canviarEstatDto.Estat)
                {
                    case EstatComanda.Aprovada:
                        comanda.DataAprovacio = DateTime.UtcNow;
                        break;
                    case EstatComanda.Finalitzada:
                        comanda.DataFinalitzacio = DateTime.UtcNow;
                        break;
                }

                await _context.SaveChangesAsync();

                // Recarregar amb includes
                comanda = await _context.Comandes
                    .Include(c => c.Usuari)
                    .Include(c => c.Linies)
                        .ThenInclude(l => l.Article)
                    .FirstOrDefaultAsync(c => c.Id == id);

                return Ok(MapToComandaDto(comanda!));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al canviar l'estat de la comanda {Id}", id);
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Elimina (desactiva) una comanda
        /// </summary>
        /// <param name="id">ID de la comanda</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")] // Només administradors poden eliminar
        public async Task<IActionResult> DeleteComanda(int id)
        {
            try
            {
                var comanda = await _context.Comandes.FindAsync(id);

                if (comanda == null)
                {
                    return NotFound("Comanda no trobada");
                }

                // Desactivar en lloc d'eliminar
                comanda.Actiu = false;
                comanda.DataModificacio = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la comanda {Id}", id);
                return StatusCode(500, "Error intern del servidor");
            }
        }

        // Mètodes auxiliars privats

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim ?? "0");
        }

        private async Task<string> GenerarNumeroComanda()
        {
            var any = DateTime.UtcNow.Year;
            var ultimaComanda = await _context.Comandes
                .Where(c => c.NumeroComanda.StartsWith($"COM-{any}-"))
                .OrderByDescending(c => c.NumeroComanda)
                .FirstOrDefaultAsync();

            int sequencial = 1;
            if (ultimaComanda != null)
            {
                var parts = ultimaComanda.NumeroComanda.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int num))
                {
                    sequencial = num + 1;
                }
            }

            return $"COM-{any}-{sequencial:D6}";
        }

        private void CalcularTotals(Comanda comanda)
        {
            comanda.Total = comanda.Linies.Sum(l => l.Total);
            comanda.ImportDescompte = comanda.Total * (comanda.DescomptePercentatge / 100);
            comanda.TotalAmbDescompte = comanda.Total - comanda.ImportDescompte;
        }

        private static ComandaDto MapToComandaDto(Comanda comanda)
        {
            return new ComandaDto
            {
                Id = comanda.Id,
                NumeroComanda = comanda.NumeroComanda,
                UsuariId = comanda.UsuariId,
                Estat = comanda.Estat,
                DataCreacio = comanda.DataCreacio,
                DataModificacio = comanda.DataModificacio,
                DataAprovacio = comanda.DataAprovacio,
                DataFinalitzacio = comanda.DataFinalitzacio,
                Observacions = comanda.Observacions,
                Total = comanda.Total,
                DescomptePercentatge = comanda.DescomptePercentatge,
                ImportDescompte = comanda.ImportDescompte,
                TotalAmbDescompte = comanda.TotalAmbDescompte,
                Actiu = comanda.Actiu,
                Linies = comanda.Linies.Select(l => new LiniaComandaDto
                {
                    Id = l.Id,
                    ComandaId = l.ComandaId,
                    ArticleId = l.ArticleId,
                    NomProducte = l.NomProducte,
                    Descripcio = l.Descripcio,
                    Quantitat = l.Quantitat,
                    PreuUnitari = l.PreuUnitari,
                    DescomptePercentatge = l.DescomptePercentatge,
                    Subtotal = l.Subtotal,
                    ImportDescompte = l.ImportDescompte,
                    Total = l.Total,
                    Ordre = l.Ordre,
                    DataCreacio = l.DataCreacio
                }).OrderBy(l => l.Ordre).ToList()
            };
        }
    }
}
