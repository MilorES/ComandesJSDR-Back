using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComandesAPI.Data;
using ComandesAPI.Models;
using ComandesAPI.DTOs;

namespace ComandesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly ComandesDbContext _context;
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(ComandesDbContext context, ILogger<ArticlesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obté tots els articles
        /// </summary>
        /// <param name="categoria">Filtre opcional per categoria</param>
        /// <param name="actius">Filtre opcional per articles actius</param>
        /// <returns>Llista d'articles</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetArticles(
            [FromQuery] string? categoria = null,
            [FromQuery] bool? actius = null)
        {
            try
            {
                var query = _context.Articles.AsQueryable();

                if (!string.IsNullOrEmpty(categoria))
                {
                    query = query.Where(a => a.Categoria == categoria);
                }

                if (actius.HasValue)
                {
                    query = query.Where(a => a.Actiu == actius.Value);
                }

                var articles = await query
                    .OrderBy(a => a.Nom)
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

                return Ok(articles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir els articles");
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Obté un article específic per ID
        /// </summary>
        /// <param name="id">ID de l'article</param>
        /// <returns>Article sol·licitat</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleDto>> GetArticle(int id)
        {
            try
            {
                var article = await _context.Articles
                    .Where(a => a.Id == id)
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
                    .FirstOrDefaultAsync();

                if (article == null)
                {
                    return NotFound($"No s'ha trobat l'article amb ID {id}");
                }

                return Ok(article);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir l'article amb ID {Id}", id);
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Crea un nou article
        /// </summary>
        /// <param name="createArticleDto">Dades del nou article</param>
        /// <returns>Article creat</returns>
        [HttpPost]
        public async Task<ActionResult<ArticleDto>> CreateArticle(CreateArticleDto createArticleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar si ja existeix un article amb el mateix nom
                var existeixArticle = await _context.Articles
                    .AnyAsync(a => a.Nom.ToLower() == createArticleDto.Nom.ToLower());

                if (existeixArticle)
                {
                    return Conflict($"Ja existeix un article amb el nom '{createArticleDto.Nom}'");
                }

                var article = new Article
                {
                    Nom = createArticleDto.Nom,
                    Descripcio = createArticleDto.Descripcio,
                    Preu = createArticleDto.Preu,
                    Estoc = createArticleDto.Estoc,
                    Categoria = createArticleDto.Categoria,
                    Actiu = createArticleDto.Actiu,
                    DataCreacio = DateTime.UtcNow
                };

                _context.Articles.Add(article);
                await _context.SaveChangesAsync();

                var articleDto = new ArticleDto
                {
                    Id = article.Id,
                    Nom = article.Nom,
                    Descripcio = article.Descripcio,
                    Preu = article.Preu,
                    Estoc = article.Estoc,
                    Categoria = article.Categoria,
                    Actiu = article.Actiu,
                    DataCreacio = article.DataCreacio,
                    DataModificacio = article.DataModificacio
                };

                return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, articleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear l'article");
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Actualitza un article existent
        /// </summary>
        /// <param name="id">ID de l'article a actualitzar</param>
        /// <param name="updateArticleDto">Noves dades de l'article</param>
        /// <returns>Article actualitzat</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ArticleDto>> UpdateArticle(int id, UpdateArticleDto updateArticleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var article = await _context.Articles.FindAsync(id);
                if (article == null)
                {
                    return NotFound($"No s'ha trobat l'article amb ID {id}");
                }

                // Verificar si el nou nom ja existeix (només si s'ha canviat)
                if (!string.IsNullOrEmpty(updateArticleDto.Nom) && 
                    updateArticleDto.Nom.ToLower() != article.Nom.ToLower())
                {
                    var existeixArticle = await _context.Articles
                        .AnyAsync(a => a.Nom.ToLower() == updateArticleDto.Nom.ToLower() && a.Id != id);

                    if (existeixArticle)
                    {
                        return Conflict($"Ja existeix un article amb el nom '{updateArticleDto.Nom}'");
                    }
                }

                // Actualitzar només els camps proporcionats
                if (!string.IsNullOrEmpty(updateArticleDto.Nom))
                    article.Nom = updateArticleDto.Nom;

                if (updateArticleDto.Descripcio != null)
                    article.Descripcio = updateArticleDto.Descripcio;

                if (updateArticleDto.Preu.HasValue)
                    article.Preu = updateArticleDto.Preu.Value;

                if (updateArticleDto.Estoc.HasValue)
                    article.Estoc = updateArticleDto.Estoc.Value;

                if (updateArticleDto.Categoria != null)
                    article.Categoria = updateArticleDto.Categoria;

                if (updateArticleDto.Actiu.HasValue)
                    article.Actiu = updateArticleDto.Actiu.Value;

                article.DataModificacio = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var articleDto = new ArticleDto
                {
                    Id = article.Id,
                    Nom = article.Nom,
                    Descripcio = article.Descripcio,
                    Preu = article.Preu,
                    Estoc = article.Estoc,
                    Categoria = article.Categoria,
                    Actiu = article.Actiu,
                    DataCreacio = article.DataCreacio,
                    DataModificacio = article.DataModificacio
                };

                return Ok(articleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualitzar l'article amb ID {Id}", id);
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Elimina un article
        /// </summary>
        /// <param name="id">ID de l'article a eliminar</param>
        /// <returns>Missatge de confirmació</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            try
            {
                var article = await _context.Articles.FindAsync(id);
                if (article == null)
                {
                    return NotFound($"No s'ha trobat l'article amb ID {id}");
                }

                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();

                return Ok($"L'article '{article.Nom}' s'ha eliminat correctament");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar l'article amb ID {Id}", id);
                return StatusCode(500, "Error intern del servidor");
            }
        }

        /// <summary>
        /// Obté les categories disponibles
        /// </summary>
        /// <returns>Llista de categories úniques</returns>
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategories()
        {
            try
            {
                var categories = await _context.Articles
                    .Where(a => !string.IsNullOrEmpty(a.Categoria))
                    .Select(a => a.Categoria!)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToListAsync();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir les categories");
                return StatusCode(500, "Error intern del servidor");
            }
        }
    }
}