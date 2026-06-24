// Services/IArbolService.cs
using AdminArboles.Models;

namespace AdminArboles.Services
{
    public interface IArbolService
    {
        Task<Arbol> RegistrarArbolAsync(Arbol arbol);
        Task<Arbol> GetArbolByIdAsync(string id);
        Task<List<Arbol>> GetArbolesByCampañaAsync(string campañaId);
        Task<Arbol> UpdateArbolAsync(Arbol arbol);
        Task<bool> DeleteArbolAsync(string id);
        Task<bool> AsignarArbolAAlumnoAsync(string arbolId, string alumnoNombre);
        Task<List<Arbol>> GetArbolesByInspectorAsync(string inspectorId);
        Task<Dictionary<string, int>> GetEstadisticasArbolesAsync(string campañaId);
        Task<byte[]> ExportArbolesToCSVAsync(string campañaId);
    }
}
