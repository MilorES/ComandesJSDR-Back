using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComandesAPI.Data;
using ComandesAPI.DTOs;
using ComandesAPI.Models;
using System.Security.Claims;

namespace ComandesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requereix autenticació per a tots els endpoints
    public class DashboardController : ControllerBase
    {
        private readonly ComandesDbContext _context;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ComandesDbContext context, ILogger<DashboardController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obté el resum de comandes desglossat per estats
        /// Usuaris normals: només les seves comandes
        /// Administradors: totes les comandes
        /// </summary>
        /// <returns>Resum de comandes</returns>
        [HttpGet("comandes/resum")]
        public async Task<ActionResult<ResumComandesDto>> GetResumComandes()
        {
            try
            {
                var userId = GetCurrentUserId();
                var isAdmin = User.IsInRole("Administrator");

                var query = _context.Comandes.AsQueryable();

                // Si no és administrador, només pot veure les seves comandes
                if (!isAdmin)
                {
                    query = query.Where(c => c.UsuariId == userId);
                }

                var resum = new ResumComandesDto
                {
                    TotalComandes = await query.CountAsync(),
                    Esborranys = await query.CountAsync(c => c.Estat == EstatComanda.Esborrany),
                    PendentsAprovacio = await query.CountAsync(c => c.Estat == EstatComanda.PendentAprovacio),
                    Aprovades = await query.CountAsync(c => c.Estat == EstatComanda.Aprovada),
                    EnProces = await query.CountAsync(c => c.Estat == EstatComanda.EnProces),
                    Enviades = await query.CountAsync(c => c.Estat == EstatComanda.Enviada),
                    Finalitzades = await query.CountAsync(c => c.Estat == EstatComanda.Finalitzada),
                    Cancellades = await query.CountAsync(c => c.Estat == EstatComanda.Cancellada)
                };

                return Ok(resum);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir el resum de comandes");
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Obté les N últimes comandes ordenades per data de creació
        /// Usuaris normals: només les seves comandes
        /// Administradors: totes les comandes
        /// </summary>
        /// <param name="limit">Nombre de comandes a retornar (per defecte 10)</param>
        /// <returns>Llista de les últimes comandes</returns>
        [HttpGet("comandes/ultimes")]
        public async Task<ActionResult<IEnumerable<ComandaDto>>> GetUltimesComandes([FromQuery] int limit = 10)
        {
            try
            {
                if (limit <= 0 || limit > 100)
                {
                    return BadRequest("El límit ha d'estar entre 1 i 100");
                }

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

                var comandes = await query
                    .OrderByDescending(c => c.DataCreacio)
                    .Take(limit)
                    .Select(c => MapToComandaDto(c))
                    .ToListAsync();

                return Ok(comandes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir les últimes comandes");
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Obté el resum de productes (total, inhabilitats, sense stock)
        /// NOMÉS PER ADMINISTRADORS
        /// </summary>
        /// <returns>Resum de productes</returns>
        [HttpGet("productes/resum")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ResumProductesDto>> GetResumProductes()
        {
            try
            {
                var totalProductes = await _context.Articles.CountAsync();
                var productesActius = await _context.Articles.CountAsync(a => a.Actiu);
                var productesInhabilitats = await _context.Articles.CountAsync(a => !a.Actiu);
                var productesSenseStock = await _context.Articles.CountAsync(a => a.Estoc == 0);

                var resum = new ResumProductesDto
                {
                    TotalProductes = totalProductes,
                    ProductesActius = productesActius,
                    ProductesInhabilitats = productesInhabilitats,
                    ProductesSenseStock = productesSenseStock
                };

                return Ok(resum);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir el resum de productes");
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Obté els N productes més nous (novetats)
        /// NOMÉS PER ADMINISTRADORS
        /// </summary>
        /// <param name="limit">Nombre de productes a retornar (per defecte 10)</param>
        /// <returns>Llista de productes novetat</returns>
        [HttpGet("productes/novetats")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetProductesNovetat([FromQuery] int limit = 10)
        {
            try
            {
                if (limit <= 0 || limit > 100)
                {
                    return BadRequest("El límit ha d'estar entre 1 i 100");
                }

                var productes = await _context.Articles
                    .Where(a => a.Actiu)
                    .OrderByDescending(a => a.DataCreacio)
                    .Take(limit)
                    .Select(a => new ArticleDto
                    {
                        Id = a.Id,
                        Nom = a.Nom,
                        Descripcio = a.Descripcio,
                        Preu = a.Preu,
                        Estoc = a.Estoc,
                        Categoria = a.Categoria,
                        Actiu = a.Actiu,
                        DataCreacio = a.DataCreacio,
                        DataModificacio = a.DataModificacio
                    })
                    .ToListAsync();

                return Ok(productes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir els productes novetat");
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Obté els productes amb baix stock (estoc menor o igual a la quantitat especificada)
        /// NOMÉS PER ADMINISTRADORS
        /// </summary>
        /// <param name="quantitat">Quantitat màxima d'estoc per considerar "baix stock" (per defecte 10)</param>
        /// <returns>Llista de productes amb baix stock</returns>
        [HttpGet("productes/baix-stock")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetProductesBaixStock([FromQuery] int quantitat = 10)
        {
            try
            {
                if (quantitat < 0 || quantitat > 1000)
                {
                    return BadRequest("La quantitat ha d'estar entre 0 i 1000");
                }

                var productes = await _context.Articles
                    .Where(a => a.Actiu && a.Estoc <= quantitat)
                    .OrderBy(a => a.Estoc)
                    .ThenBy(a => a.Nom)
                    .Select(a => new ArticleDto
                    {
                        Id = a.Id,
                        Nom = a.Nom,
                        Descripcio = a.Descripcio,
                        Preu = a.Preu,
                        Estoc = a.Estoc,
                        Categoria = a.Categoria,
                        Actiu = a.Actiu,
                        DataCreacio = a.DataCreacio,
                        DataModificacio = a.DataModificacio
                    })
                    .ToListAsync();

                return Ok(productes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir els productes amb baix stock");
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Obté els productes agrupats per categoria amb el recompte de productes per categoria
        /// NOMÉS PER ADMINISTRADORS
        /// </summary>
        /// <returns>Llista de categories amb el nombre de productes</returns>
        [HttpGet("productes/per-categoria")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<ProductePerCategoriaDto>>> GetProductesPerCategoria()
        {
            try
            {
                var productesPerCategoria = await _context.Articles
                    .Where(a => a.Actiu)
                    .GroupBy(a => a.Categoria ?? "Sense categoria")
                    .Select(g => new ProductePerCategoriaDto
                    {
                        Categoria = g.Key,
                        QuantitatProductes = g.Count()
                    })
                    .OrderByDescending(x => x.QuantitatProductes)
                    .ThenBy(x => x.Categoria)
                    .ToListAsync();

                return Ok(productesPerCategoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir els productes per categoria");
                return StatusCode(500, "Error intern del servidor");
            }
        }

        // Mètodes auxiliars privats
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim ?? "0");
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
