// Services/ICampañaService.cs
using AdminArboles.Models;

namespace AdminArboles.Services
{
    public interface ICampañaService
    {
        Task<List<Campaña>> GetCampañasAsync(string tenantId);
        Task<Campaña> GetCampañaByIdAsync(string id);
        Task<Campaña> CreateCampañaAsync(Campaña campaña);
        Task<Campaña> UpdateCampañaAsync(Campaña campaña);
        Task<bool> DeleteCampañaAsync(string id);
        Task<string> GenerateCodigoAlfanumericoAsync(string tenantId);
        Task<List<Campaña>> GetCampañasByCoordinadorAsync(string coordinadorId);
        Task<byte[]> ExportCampañaToCSVAsync(string campañaId);
        Task<byte[]> ExportAllCampañasToCSVAsync(string tenantId);
    }
}