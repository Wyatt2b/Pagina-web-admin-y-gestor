// Services/IAuthService.cs
using AdminArboles.Models;

namespace AdminArboles.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string email, string otp);
        Task<bool> LogoutAsync();
        Task<Usuario> GetCurrentUserAsync();
        Task<bool> IsAuthenticatedAsync();
        Task<string> GetUserRoleAsync();
        Task<string> GetTenantIdAsync();
        Task<bool> ValidateOTPAsync(string email, string otp);
        Task<Usuario> GetUserByEmailAsync(string email);
    }
}