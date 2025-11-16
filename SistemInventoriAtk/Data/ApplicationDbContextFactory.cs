using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SistemInventoriAtk.Data
{
    // Class ini diperlukan agar dotnet ef tools dapat membuat instance DbContext 
    // saat menjalankan commands (migrations, database update).
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Ambil path ke file appsettings.json di root project
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Dapatkan Connection String
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                // Jika Connection String kosong, berikan pesan error yang jelas
                throw new InvalidOperationException("Connection string 'DefaultConnection' tidak ditemukan di appsettings.json.");
            }

            // Konfigurasi DbContextOptions menggunakan provider Npgsql (PostgreSQL)
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            // Buat instance DbContext
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}