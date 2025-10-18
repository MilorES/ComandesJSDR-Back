using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ComandesAPI.Data;
using ComandesAPI.DTOs;
using ComandesAPI.Services;

namespace ComandesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly ComandesDbContext _context;

        public AuthController(IAuthService authService, IJwtService jwtService, ComandesDbContext context)
        {
            _authService = authService;
            _jwtService = jwtService;
            _context = context;
        }

        /// <summary>
        /// Endpoint de login per autenticar usuaris
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            // Validar les dades d'entrada
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dades d'entrada no vàlides" });
            }

            // Autenticar l'usuari
            var user = await _authService.AuthenticateAsync(request.Username, request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Credencials incorrectes o usuari deshabilitat" });
            }

            // Generar el token JWT
            var token = _jwtService.GenerateToken(user);

            var response = new LoginResponseDto
            {
                Token = token,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            return Ok(response);
        }

        /// <summary>
        /// Endpoint per obtenir informació de l'usuari autenticat actual
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            // Obtenir l'ID de l'usuari del token JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Token invàlid" });
            }

            var user = await _context.Usuaris.FindAsync(userId);

            if (user == null || !user.IsEnabled)
            {
                return NotFound(new { message = "Usuari no trobat o deshabilitat" });
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
        /// Endpoint per refrescar el token JWT
        /// </summary>
        [HttpPost("refresh")]
        [Authorize]
        public async Task<ActionResult<LoginResponseDto>> RefreshToken()
        {
            // Obtenir l'ID de l'usuari del token actual
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Token invàlid" });
            }

            var user = await _context.Usuaris.FindAsync(userId);

            if (user == null || !user.IsEnabled)
            {
                return NotFound(new { message = "Usuari no trobat o deshabilitat" });
            }

            // Generar un nou token
            var token = _jwtService.GenerateToken(user);

            var response = new LoginResponseDto
            {
                Token = token,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            return Ok(response);
        }

        /// <summary>
        /// Endpoint per canviar la contrasenya de l'usuari autenticat
        /// </summary>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dades d'entrada no vàlides" });
            }

            // Obtenir l'ID de l'usuari del token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Token invàlid" });
            }

            var user = await _context.Usuaris.FindAsync(userId);

            if (user == null)
            {
                return NotFound(new { message = "Usuari no trobat" });
            }

            // Verificar la contrasenya actual
            if (!_authService.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                return BadRequest(new { message = "La contrasenya actual no és correcta" });
            }

            // Actualitzar la contrasenya
            user.PasswordHash = _authService.HashPassword(request.NewPassword);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Contrasenya canviada correctament" });
        }
    }
}
