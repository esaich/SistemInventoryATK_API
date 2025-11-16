using Microsoft.AspNetCore.Mvc;
using SistemInventoriAtk.Services;

namespace SistemInventoriAtk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))] // Mengembalikan Token (string)
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Memanggil service untuk memproses login dan generate token
            var token = await _authService.LoginAsync(request);

            if (token == null)
            {
                // Kredensial tidak valid
                return Unauthorized(new { message = "Email atau Password tidak valid." });
            }

            // Login berhasil, kembalikan token
            return Ok(new { token = token });
        }
    }
}