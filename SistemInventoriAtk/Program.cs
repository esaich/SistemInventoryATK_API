using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SistemInventoriAtk.Data;
using SistemInventoriAtk.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// --- 1. Konfigurasi Database (PostgreSQL) ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

// --- 2. Registrasi Services (DI) ---
// Mendaftarkan AuthService untuk digunakan di Controller
builder.Services.AddScoped<IAuthService, AuthService>();

// --- 3. Konfigurasi JWT Authentication ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? string.Empty))
        };
    });

// --- 4. Konfigurasi Authorization (Policies) ---
builder.Services.AddAuthorization(options =>
{
    // Kebijakan untuk Admin (hanya role "admin")
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("admin"));

    // Kebijakan untuk Divisi (hanya role "divisi")
    options.AddPolicy("DivisiPolicy", policy =>
        policy.RequireRole("divisi"));
});

// --- 5. Konfigurasi Bawaan (Controllers & Swagger) ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// --- 6. Konfigurasi Swagger untuk JWT ---
// Ini menambahkan tombol "Authorize" di Swagger UI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sistem Inventori ATK API", Version = "v1" });

    // Menambahkan definisi keamanan (Bearer Token)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
                      "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                      "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// --- Build Aplikasi ---
var app = builder.Build();

// --- Konfigurasi HTTP Request Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- 7. Aktifkan Authentication & Authorization ---
// PENTING: app.UseAuthentication() HARUS sebelum app.UseAuthorization()
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();