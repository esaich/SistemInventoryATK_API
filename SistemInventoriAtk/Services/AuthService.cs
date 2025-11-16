using SistemInventoriAtk.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SistemInventoriAtk.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string?> LoginAsync(AuthRequest request)
        {
            // 1. Cari user di database
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return null; // User tidak ditemukan
            }

            // 2. Validasi Password (Saat ini menggunakan perbandingan plain text. 
            //    Dalam aplikasi nyata, ini HARUS diganti dengan hashing, misal: BCrypt.Net)
            if (request.Password != user.Password) // Contoh Sederhana
            {
                // TODO: Ganti ini dengan pengecekan hash password
                return null; // Password salah
            }

            // 3. Generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();

            // Ambil Secret Key dari konfigurasi
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ??
                                             throw new InvalidOperationException("Jwt:Key tidak terkonfigurasi."));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    // Claim Id dan Email
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    
                    // Claim Role (Ini yang paling penting untuk otorisasi)
                    new Claim(ClaimTypes.Role, user.Role)
                }),

                // Konfigurasi Token
                Expires = DateTime.UtcNow.AddDays(7), // Token berlaku 7 hari
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}