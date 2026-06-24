// Services/IInspectorService.cs
using AdminArboles.Models;

namespace AdminArboles.Services
{
    public interface IInspectorService
    {
        Task<Inspector> RegistrarInspectorAsync(Inspector inspector);
        Task<Inspector> GetInspectorByIdAsync(string id);
        Task<List<Inspector>> GetInspectoresByCampañaAsync(string campañaId);
        Task<Inspector> UpdateInspectorAsync(Inspector inspector);
        Task<bool> DeleteInspectorAsync(string id);
        Task<bool> ElevarPrivilegiosInspectorAsync(string inspectorId);
        Task<Inspector> GetInspectorByUsuarioIdAsync(string usuarioId);
        Task<List<Inspector>> GetInspectoresByTenantAsync(string tenantId);
    }
}
