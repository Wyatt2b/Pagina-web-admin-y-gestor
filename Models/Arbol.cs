// Models/Arbol.cs
namespace AdminArboles.Models
{
    public class Arbol
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Especie { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public double Altura { get; set; }
        public double Diametro { get; set; }
        public DateTime FechaPlantacion { get; set; }
        public DateTime FechaUltimaMedicion { get; set; }
        public string Estado { get; set; } = "Vivo"; // Vivo, Enfermo, Muerto
        public string CampañaId { get; set; } = string.Empty;
        public string InspectorId { get; set; } = string.Empty;
        public string AlumnoAsignado { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
    }
}