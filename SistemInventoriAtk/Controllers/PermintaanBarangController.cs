using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemInventoriAtk.Data;
using SistemInventoriAtk.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SistemInventoriAtk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermintaanBarangController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PermintaanBarangController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===================================================
        // POST: api/permintaanbarang
        // User mengajukan permintaan barang
        // ===================================================
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Create([FromBody] PermintaanBarang model)
        {
            var barang = await _context.Barang.FindAsync(model.BarangId);
            if (barang == null)
                return BadRequest(new { message = "Barang tidak ditemukan." });

            // Tidak perlu langsung kurangi stok di sini
            // Stok akan berkurang saat admin menyetujui

            model.Status = "pending";
            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            _context.PermintaanBarang.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Permintaan barang berhasil diajukan." });
        }

        // ===================================================
        // GET: api/permintaanbarang
        // Admin & User bisa melihat semua permintaan
        // ===================================================
        [HttpGet]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.PermintaanBarang
                .Include(p => p.User)
                .Include(p => p.Barang)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new
                {
                    p.Id,
                    User = p.User.Name,
                    Barang = p.Barang.NamaBarang,
                    p.Jumlah,
                    p.Status,
                    p.Alasan,
                    p.CreatedAt,
                    p.UpdatedAt
                })
                .ToListAsync();

            return Ok(data);
        }

        // ===================================================
        // PUT: api/permintaanbarang/{id}
        // User bisa edit selama pending
        // Admin bisa edit kapanpun
        // ===================================================
        [HttpPut("{id}")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> Update(int id, [FromBody] PermintaanBarang model)
        {
            var permintaan = await _context.PermintaanBarang.FindAsync(id);
            if (permintaan == null)
                return NotFound(new { message = "Permintaan tidak ditemukan." });

            var role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

            if (role == "user" && permintaan.Status != "pending")
                return BadRequest(new { message = "Tidak dapat mengedit, status sudah diproses." });

            permintaan.BarangId = model.BarangId;
            permintaan.Jumlah = model.Jumlah;
            permintaan.Alasan = model.Alasan;
            permintaan.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = $"Permintaan berhasil diperbarui oleh {role}." });
        }

        // ===================================================
        // DELETE: api/permintaanbarang/{id}
        // User hanya bisa hapus permintaan pending
        // ===================================================
        [HttpDelete("{id}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Delete(int id)
        {
            var permintaan = await _context.PermintaanBarang.FindAsync(id);
            if (permintaan == null)
                return NotFound(new { message = "Permintaan tidak ditemukan." });

            if (permintaan.Status != "pending")
                return BadRequest(new { message = "Tidak bisa dihapus karena sudah diproses." });

            _context.PermintaanBarang.Remove(permintaan);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Permintaan barang berhasil dihapus." });
        }

        // ===================================================
        // PUT: api/permintaanbarang/{id}/setujui
        // Admin menyetujui permintaan barang
        // ===================================================
        [HttpPut("{id}/setujui")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var permintaan = await _context.PermintaanBarang
                .Include(p => p.Barang)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (permintaan == null)
                return NotFound(new { message = "Permintaan tidak ditemukan." });

            if (permintaan.Barang.Stok < permintaan.Jumlah)
                return BadRequest(new { message = "Stok barang tidak mencukupi." });

            // Kurangi stok barang
            permintaan.Barang.Stok -= permintaan.Jumlah;

            // Ubah status
            permintaan.Status = "disetujui";
            permintaan.UpdatedAt = DateTime.UtcNow;

            // Tambahkan catatan ke BarangKeluar
            var barangKeluar = new BarangKeluar
            {
                PermintaanId = permintaan.Id,
                BarangId = permintaan.BarangId,
                JumlahKeluar = permintaan.Jumlah,
                TanggalKeluar = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Keterangan = "Permintaan barang disetujui dan barang dikeluarkan."
            };

            _context.BarangKeluar.Add(barangKeluar);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Permintaan disetujui dan barang keluar berhasil dicatat." });
        }

        // ===================================================
        // PUT: api/permintaanbarang/{id}/tolak
        // Admin menolak permintaan barang
        // ===================================================
        [HttpPut("{id}/tolak")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Reject(int id, [FromBody] string alasan)
        {
            var permintaan = await _context.PermintaanBarang.FindAsync(id);
            if (permintaan == null)
                return NotFound(new { message = "Permintaan tidak ditemukan." });

            permintaan.Status = "ditolak";
            permintaan.Alasan = alasan;
            permintaan.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Permintaan barang telah ditolak." });
        }
    }
}
