using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemInventoriAtk.Data;
using SistemInventoriAtk.Models;

namespace SistemInventoriAtk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ======================================================
        // GET: api/payment
        // ======================================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _context.Payments
                .Include(p => p.Supplier)
                .OrderByDescending(p => p.TanggalBayar)
                .Select(p => new
                {
                    p.Id,
                    Supplier = p.Supplier.NamaSupplier,
                    p.TotalHarga,
                    p.TanggalBayar,
                    p.Keterangan,
                    p.CreatedAt,
                    p.UpdatedAt
                })
                .ToListAsync();

            return Ok(payments);
        }

        // ======================================================
        // GET: api/payment/{id}
        // ======================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var payment = await _context.Payments
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
                return NotFound(new { message = "Data pembayaran tidak ditemukan." });

            return Ok(payment);
        }

        // ======================================================
        // POST: api/payment
        // ======================================================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Payment model)
        {
            var supplier = await _context.Suppliers.FindAsync(model.SupplierId);
            if (supplier == null)
                return BadRequest(new { message = "Supplier tidak ditemukan." });

            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            _context.Payments.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Pembayaran ke supplier berhasil dicatat." });
        }

        // ======================================================
        // PUT: api/payment/{id}
        // ======================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Payment model)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return NotFound(new { message = "Data pembayaran tidak ditemukan." });

            var supplier = await _context.Suppliers.FindAsync(model.SupplierId);
            if (supplier == null)
                return BadRequest(new { message = "Supplier tidak ditemukan." });

            payment.SupplierId = model.SupplierId;
            payment.TotalHarga = model.TotalHarga;
            payment.TanggalBayar = model.TanggalBayar;
            payment.Keterangan = model.Keterangan;
            payment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Data pembayaran berhasil diperbarui." });
        }

        // ======================================================
        // DELETE: api/payment/{id}
        // ======================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return NotFound(new { message = "Data pembayaran tidak ditemukan." });

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Data pembayaran berhasil dihapus." });
        }
    }
}
