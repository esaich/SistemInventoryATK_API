using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemInventoriAtk.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Barang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KodeBarang = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NamaBarang = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Stok = table.Column<int>(type: "integer", nullable: false),
                    Satuan = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barang", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NamaSupplier = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Alamat = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Telepon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BarangMasuk",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BarangId = table.Column<int>(type: "integer", nullable: false),
                    SupplierId = table.Column<int>(type: "integer", nullable: true),
                    JumlahMasuk = table.Column<int>(type: "integer", nullable: false),
                    HargaSatuan = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    TanggalMasuk = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarangMasuk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BarangMasuk_Barang_BarangId",
                        column: x => x.BarangId,
                        principalTable: "Barang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BarangMasuk_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SupplierId = table.Column<int>(type: "integer", nullable: false),
                    TotalHarga = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    TanggalBayar = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Keterangan = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PengadaanBarang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NamaBarang = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Satuan = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    JumlahDiajukan = table.Column<int>(type: "integer", nullable: false),
                    TanggalPengajuan = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Keterangan = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SupplierId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PengadaanBarang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PengadaanBarang_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermintaanBarang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    BarangId = table.Column<int>(type: "integer", nullable: false),
                    Jumlah = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Alasan = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermintaanBarang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermintaanBarang_Barang_BarangId",
                        column: x => x.BarangId,
                        principalTable: "Barang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermintaanBarang_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BarangKeluar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PermintaanId = table.Column<int>(type: "integer", nullable: false),
                    BarangId = table.Column<int>(type: "integer", nullable: false),
                    JumlahKeluar = table.Column<int>(type: "integer", nullable: false),
                    TanggalKeluar = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Keterangan = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarangKeluar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BarangKeluar_Barang_BarangId",
                        column: x => x.BarangId,
                        principalTable: "Barang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BarangKeluar_PermintaanBarang_PermintaanId",
                        column: x => x.PermintaanId,
                        principalTable: "PermintaanBarang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Barang_KodeBarang",
                table: "Barang",
                column: "KodeBarang",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BarangKeluar_BarangId",
                table: "BarangKeluar",
                column: "BarangId");

            migrationBuilder.CreateIndex(
                name: "IX_BarangKeluar_PermintaanId",
                table: "BarangKeluar",
                column: "PermintaanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BarangMasuk_BarangId",
                table: "BarangMasuk",
                column: "BarangId");

            migrationBuilder.CreateIndex(
                name: "IX_BarangMasuk_SupplierId",
                table: "BarangMasuk",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SupplierId",
                table: "Payments",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PengadaanBarang_SupplierId",
                table: "PengadaanBarang",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PermintaanBarang_BarangId",
                table: "PermintaanBarang",
                column: "BarangId");

            migrationBuilder.CreateIndex(
                name: "IX_PermintaanBarang_UserId",
                table: "PermintaanBarang",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BarangKeluar");

            migrationBuilder.DropTable(
                name: "BarangMasuk");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PengadaanBarang");

            migrationBuilder.DropTable(
                name: "PermintaanBarang");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Barang");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
