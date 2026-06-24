// Services/ITenantService.cs
using AdminArboles.Models;

namespace AdminArboles.Services
{
    public interface ITenantService
    {
        Task<Tenant> CrearTenantAsync(Tenant tenant);
        Task<Tenant> GetTenantByIdAsync(string id);
        Task<List<Tenant>> GetAllTenantsAsync();
        Task<Tenant> UpdateTenantAsync(Tenant tenant);
        Task<bool> DeleteTenantAsync(string id);
        Task<bool> AsignarCoordinadorAsync(string tenantId, string coordinadorId);
        Task<bool> RemoverCoordinadorAsync(string tenantId, string coordinadorId);
        Task<List<Usuario>> GetCoordinadoresPorTenantAsync(string tenantId);
    }
}