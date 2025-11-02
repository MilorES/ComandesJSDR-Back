using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComandesAPI.Data;
using ComandesAPI.DTOs;
using ComandesAPI.Models;
using ComandesAPI.Services;

namespace ComandesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")] // Només accessibles per administradors
    public class UsersController : ControllerBase
    {
        private readonly ComandesDbContext _context;
        private readonly IAuthService _authService;

        public UsersController(ComandesDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        /// <summary>
        /// Obtenir tots els usuaris (només administradors)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _context.Usuaris
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    FullName = u.FullName,
                    Email = u.Email,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt,
                    IsEnabled = u.IsEnabled
                })
                .ToListAsync();

            return Ok(users);
        }

        /// <summary>
        /// Obtenir un usuari per ID (només administradors)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Usuaris.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "Usuari no trobat" });
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                IsEnabled = user.IsEnabled
            };

            return Ok(userDto);
        }

        /// <summary>
        /// Crear un nou usuari (només administradors)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dades d'entrada no vàlides", errors = ModelState });
            }

            // Verificar si el username ja existeix
            if (await _context.Usuaris.AnyAsync(u => u.Username == createDto.Username))
            {
                return Conflict(new { message = "El nom d'usuari ja existeix" });
            }

            // Verificar si l'email ja existeix
            if (await _context.Usuaris.AnyAsync(u => u.Email == createDto.Email))
            {
                return Conflict(new { message = "L'email ja està en ús" });
            }

            // Crear el nou usuari
            var user = new Usuari
            {
                Username = createDto.Username,
                PasswordHash = _authService.HashPassword(createDto.Password),
                FullName = createDto.FullName,
                Email = createDto.Email,
                Role = createDto.Role,
                IsEnabled = createDto.IsEnabled,
                CreatedAt = DateTime.UtcNow
            };

            _context.Usuaris.Add(user);
            await _context.SaveChangesAsync();

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                IsEnabled = user.IsEnabled
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
        }

        /// <summary>
        /// Actualitzar un usuari existent (només administradors)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dades d'entrada no vàlides", errors = ModelState });
            }

            var user = await _context.Usuaris.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "Usuari no trobat" });
            }

            // Actualitzar només els camps proporcionats
            if (!string.IsNullOrEmpty(updateDto.FullName))
            {
                user.FullName = updateDto.FullName;
            }

            if (!string.IsNullOrEmpty(updateDto.Email))
            {
                // Verificar si el nou email ja està en ús per un altre usuari
                if (await _context.Usuaris.AnyAsync(u => u.Email == updateDto.Email && u.Id != id))
                {
                    return Conflict(new { message = "L'email ja està en ús per un altre usuari" });
                }
                user.Email = updateDto.Email;
            }

            if (!string.IsNullOrEmpty(updateDto.Role))
            {
                user.Role = updateDto.Role;
            }

            if (updateDto.IsEnabled.HasValue)
            {
                user.IsEnabled = updateDto.IsEnabled.Value;
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuari actualitzat correctament" });
        }

        /// <summary>
        /// Eliminar un usuari (només administradors)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Usuaris.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "Usuari no trobat" });
            }

            // Evitar que l'administrador s'elimini a si mateix
            var currentUsername = User.Identity?.Name;
            if (user.Username == currentUsername)
            {
                return BadRequest(new { message = "No pots eliminar el teu propi compte d'usuari" });
            }

            _context.Usuaris.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuari eliminat correctament" });
        }
    }
}
