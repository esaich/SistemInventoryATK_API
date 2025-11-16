using System.ComponentModel.DataAnnotations;

namespace SistemInventoriAtk.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(AuthRequest request);
    }
}
