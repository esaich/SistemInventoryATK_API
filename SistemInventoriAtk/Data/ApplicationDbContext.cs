using Microsoft.EntityFrameworkCore;
using SistemInventoriAtk.Models; // <-- Namespace Models yang diperbaiki

namespace SistemInventoriAtk.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Definisi semua DbSet (Tabel)
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Supplier> Suppliers { get; set; } = null!; // Menggunakan Suppliers (Plural)
        public DbSet<Barang> Barang { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<BarangMasuk> BarangMasuk { get; set; } = null!;
        public DbSet<BarangKeluar> BarangKeluar { get; set; } = null!;
        public DbSet<PermintaanBarang> PermintaanBarang { get; set; } = null!;
        public DbSet<PengadaanBarang> PengadaanBarang { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -------------------------------------------------------------------
            // Konfigurasi Constraints (UNIQUE Keys & Default Values)
            // -------------------------------------------------------------------

            // Tabel User: email unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Tabel Barang: kode_barang unique
            modelBuilder.Entity<Barang>()
                .HasIndex(b => b.KodeBarang)
                .IsUnique();

            // Atur default value dan timestamps (Laravel style)
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("NOW()");

            // -------------------------------------------------------------------
            // Konfigurasi Relasi & Foreign Key Actions (onDelete)
            // -------------------------------------------------------------------

            // BarangKeluar (One-to-One ke PermintaanBarang)
            modelBuilder.Entity<BarangKeluar>()
                .HasOne(bk => bk.Permintaan)
                .WithOne(pb => pb.BarangKeluar)
                .HasForeignKey<BarangKeluar>(bk => bk.PermintaanId)
                .OnDelete(DeleteBehavior.Cascade); // onDelete('cascade')

            // PermintaanBarang
            modelBuilder.Entity<PermintaanBarang>()
                .HasOne(pb => pb.User)
                .WithMany(u => u.PermintaanBarang)
                .OnDelete(DeleteBehavior.Cascade); // onDelete('cascade')

            // BarangMasuk
            modelBuilder.Entity<BarangMasuk>()
                .HasOne(bm => bm.Supplier)
                .WithMany() // Tidak ada navigation property di Supplier ke BarangMasuk
                .OnDelete(DeleteBehavior.SetNull); // onDelete('set null')

            // Payment
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Payments)
                .OnDelete(DeleteBehavior.Cascade); // onDelete('cascade')

            // PengadaanBarang
            modelBuilder.Entity<PengadaanBarang>()
                .HasOne(pb => pb.Supplier)
                .WithMany(s => s.PengadaanBarang)
                .OnDelete(DeleteBehavior.Cascade); // onDelete('cascade')
        }
    }
}