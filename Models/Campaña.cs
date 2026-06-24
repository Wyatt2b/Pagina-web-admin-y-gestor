// Models/Campaña.cs
namespace AdminArboles.Models
{
    public class Campaña
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string CodigoAlfanumerico { get; set; } = string.Empty;
        public string CoordinadorId { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int ArbolesPlantados { get; set; }
        public int ArbolesSobrevivientes { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public List<Arbol> Arboles { get; set; } = new();
    }
}