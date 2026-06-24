// Models/Tenant.cs
namespace AdminArboles.Models
{
    public class Tenant
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Nombre { get; set; } = string.Empty;
        public string Dominio { get; set; } = string.Empty;
        public string Plan { get; set; } = "Básico";
        public bool IsActive { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public List<string> CoordinadoresIds { get; set; } = new();
        public List<Campaña> Campañas { get; set; } = new();
    }
}