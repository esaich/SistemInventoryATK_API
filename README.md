# SistemInventoryATK_API
# Sistem Inventori ATK – ASP.NET Core Web API

Sistem Inventori ATK adalah aplikasi backend berbasis **ASP.NET Core Web API**
yang digunakan untuk mengelola persediaan Alat Tulis Kantor (ATK), Aplikasi ini menyediakan fitur pengelolaan barang, permintaan,
pengadaan, supplier, hingga pencatatan barang masuk & keluar.

Seluruh proses otentikasi menggunakan **JWT (JSON Web Token)** dan akses
diatur menggunakan **Role-Based Authorization** (Admin & User).

---

## ✨ Fitur Utama

### 🔐 Autentikasi
- Login menggunakan email dan password
- JWT Authentication
- Role-based Authorization: `admin` dan `user`

### 📦 Manajemen Barang
- CRUD data barang
- Validasi kode barang unik
- Menampilkan stok terbaru

### 📥 Barang Masuk
- Input barang hasil pengadaan
- Update stok otomatis

### 📤 Barang Keluar
- Input barang keluar manual / via permintaan
- Validasi stok mencukupi
- Update stok otomatis

### 🧾 Permintaan Barang
- Divisi mengajukan permintaan
- Admin menyetujui / menolak

### 🏷️ Supplier
- CRUD supplier

### 🧾 Pengadaan
- Pencatatan pengadaan barang
- Relasi barang & supplier

### 👤 Manajemen User (Admin Only)
- Create user baru (admin / user)
- Lihat semua user

---

## 🏛️ Arsitektur

Aplikasi menggunakan pola berikut:

- **ASP.NET Core Web API**
- **Entity Framework Core 8**
- Database: **PostgreSQL**
- **JWT Authentication**
- Layer:
  - Controller → DbContext (tanpa repository, simple architecture)
- Swagger untuk dokumentasi API


---

## 🛠️ Teknologi yang Digunakan

- .NET 8 Web API
- Entity Framework Core 8
- PostgreSQL
- Swagger / Swashbuckle
- BCrypt.Net
- JWT (System.IdentityModel.Tokens.Jwt)
- Visual Studio 2022 / VS Code

---

## 🚀 Cara Instalasi & Menjalankan API

### 1. Clone Repository
bash
git clone https://github.com/username/SistemInventoriAtk.git
cd SistemInventoriAtk

---
### 2. Clone Repository
dotnet restore
### 3. Sesuaikan database (PostgreSQL)
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=123"
}

### 4. Generate database
dotnet ef database update

### 5. Jalankan aplikasi
dotnet run
### API Akan Berjalan di :
http://localhost:5046

### Environment Variables

Pastikan JWT key minimal 32 karakter:

"Jwt": {
  "Issuer": "SistemInventoriAtkAPI",
  "Audience": "SistemInventoriAtkClient",
  "Key": "IniAdalahKunciSuperRahasiaUntukJWTAndaMinimal32Karakter"
}

### 📚 API Endpoint List
🔐 Auth
Method	Endpoint	Deskripsi
POST	/api/auth/login	Login & generate JWT
👤 Admin
Method	Endpoint	Deskripsi
POST	/api/admin/createuser	Buat user baru
GET	/api/admin/users	List user
📦 Barang
Method	Endpoint	Deskripsi	Auth
GET	/api/barang	List semua barang	Admin/User
GET	/api/barang/{id}	Detail barang	Admin/User
POST	/api/barang	Tambah barang	Admin
PUT	/api/barang/{id}	Update barang	Admin
DELETE	/api/barang/{id}	Hapus barang	Admin
📤 Barang Keluar
Method	Endpoint	Deskripsi
GET	/api/barangkeluar	Semua barang keluar
POST	/api/barangkeluar	Catat barang keluar
DELETE	/api/barangkeluar/{id}	Hapus data
🧾 Permintaan Barang
Method	Endpoint	Deskripsi
GET	/api/permintaanbarang	List permintaan
POST	/api/permintaanbarang	Buat permintaan
PUT	/api/permintaanbarang/{id}/approve	Setujui
PUT	/api/permintaanbarang/{id}/reject	Tolak
🏷️ Supplier
Method	Endpoint	Deskripsi
GET	/api/supplier	List supplier
POST	/api/supplier	Tambah supplier
