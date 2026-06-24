// Models/Usuario.cs
namespace AdminArboles.Models
{
    public class Usuario
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Rol { get; set; } = "Coordinador"; // Coordinador, Admin, Inspector
        public string TenantId { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
