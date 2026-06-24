// Services/MockServices.cs (Implementación con datos en memoria)
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using AdminArboles.Models;

namespace AdminArboles.Services
{
    public class MockAuthService : IAuthService
    {
        private readonly List<Usuario> _usuarios = new();
        private Usuario? _currentUser;

        public MockAuthService()
        {
            // Datos de ejemplo
            _usuarios.Add(new Usuario 
            { 
                Id = "1", 
                Email = "coordinador@ejemplo.com", 
                Nombre = "Coordinador Test", 
                Rol = "Coordinador",
                Telefono = "123456789",
                TenantId = "tenant1"
            });
            _usuarios.Add(new Usuario 
            { 
                Id = "2", 
                Email = "admin@ejemplo.com", 
                Nombre = "Admin Test", 
                Rol = "Admin",
                Telefono = "987654321",
                TenantId = "tenant1"
            });
        }

        public async Task<bool> LoginAsync(string email, string otp)
        {
            var usuario = await GetUserByEmailAsync(email);
            if (usuario == null) return false;

            // Validación simple: OTP debe ser "123456" para este demo
            if (otp != "123456") return false;

            _currentUser = usuario;
            return true;
        }

        public async Task<bool> LogoutAsync()
        {
            _currentUser = null;
            return await Task.FromResult(true);
        }

        public async Task<Usuario> GetCurrentUserAsync()
        {
            return await Task.FromResult(_currentUser);
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            return await Task.FromResult(_currentUser != null);
        }

        public async Task<string> GetUserRoleAsync()
        {
            return await Task.FromResult(_currentUser?.Rol ?? "Invited");
        }

        public async Task<string> GetTenantIdAsync()
        {
            return await Task.FromResult(_currentUser?.TenantId ?? string.Empty);
        }

        public async Task<bool> ValidateOTPAsync(string email, string otp)
        {
            var usuario = await GetUserByEmailAsync(email);
            if (usuario == null) return false;
            return otp == "123456"; // Demo: OTP fijo
        }

        public async Task<Usuario> GetUserByEmailAsync(string email)
        {
            return await Task.FromResult(_usuarios.FirstOrDefault(u => u.Email == email));
        }
    }

    public class MockCampañaService : ICampañaService
    {
        private readonly List<Campaña> _campañas = new();
        private readonly Random _random = new();

        public MockCampañaService()
        {
            // Datos de ejemplo
            _campañas.Add(new Campaña
            {
                Id = "1",
                Nombre = "Campaña Primavera 2026",
                Descripcion = "Plantación de árboles nativos en el parque central",
                CodigoAlfanumerico = "CP-2026-001",
                CoordinadorId = "1",
                TenantId = "tenant1",
                FechaInicio = DateTime.Now.AddDays(-30),
                FechaFin = DateTime.Now.AddDays(30),
                ArbolesPlantados = 150,
                ArbolesSobrevivientes = 142,
                Ubicacion = "Parque Central"
            });
        }

        public async Task<List<Campaña>> GetCampañasAsync(string tenantId)
        {
            return await Task.FromResult(_campañas
                .Where(c => c.TenantId == tenantId)
                .ToList());
        }

        public async Task<Campaña> GetCampañaByIdAsync(string id)
        {
            return await Task.FromResult(_campañas.FirstOrDefault(c => c.Id == id));
        }

        public async Task<Campaña> CreateCampañaAsync(Campaña campaña)
        {
            campaña.Id = Guid.NewGuid().ToString();
            campaña.CodigoAlfanumerico = await GenerateCodigoAlfanumericoAsync(campaña.TenantId);
            _campañas.Add(campaña);
            return await Task.FromResult(campaña);
        }

        public async Task<Campaña> UpdateCampañaAsync(Campaña campaña)
        {
            var index = _campañas.FindIndex(c => c.Id == campaña.Id);
            if (index != -1)
            {
                _campañas[index] = campaña;
            }
            return await Task.FromResult(campaña);
        }

        public async Task<bool> DeleteCampañaAsync(string id)
        {
            var campaña = _campañas.FirstOrDefault(c => c.Id == id);
            if (campaña != null)
            {
                campaña.IsActive = false;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<string> GenerateCodigoAlfanumericoAsync(string tenantId)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var code = new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
            
            return await Task.FromResult($"CP-{DateTime.Now.Year}-{code}");
        }

        public async Task<List<Campaña>> GetCampañasByCoordinadorAsync(string coordinadorId)
        {
            return await Task.FromResult(_campañas
                .Where(c => c.CoordinadorId == coordinadorId)
                .ToList());
        }

        public async Task<byte[]> ExportCampañaToCSVAsync(string campañaId)
        {
            var campaña = await GetCampañaByIdAsync(campañaId);
            if (campaña == null) return Array.Empty<byte>();

            var csv = new StringBuilder();
            csv.AppendLine("ID,Campaña,Código,Ubicación,Fecha Inicio,Fecha Fin,Árboles Plantados,Supervivencia");
            csv.AppendLine($"{campaña.Id},{campaña.Nombre},{campaña.CodigoAlfanumerico},{campaña.Ubicacion},{campaña.FechaInicio:yyyy-MM-dd},{campaña.FechaFin:yyyy-MM-dd},{campaña.ArbolesPlantados},{campaña.ArbolesSobrevivientes}");

            return Encoding.UTF8.GetBytes(csv.ToString());
        }

        public async Task<byte[]> ExportAllCampañasToCSVAsync(string tenantId)
        {
            var campañas = await GetCampañasAsync(tenantId);
            var csv = new StringBuilder();
            csv.AppendLine("ID,Campaña,Código,Ubicación,Fecha Inicio,Fecha Fin,Árboles Plantados,Supervivencia");
            
            foreach (var c in campañas)
            {
                csv.AppendLine($"{c.Id},{c.Nombre},{c.CodigoAlfanumerico},{c.Ubicacion},{c.FechaInicio:yyyy-MM-dd},{c.FechaFin:yyyy-MM-dd},{c.ArbolesPlantados},{c.ArbolesSobrevivientes}");
            }

            return Encoding.UTF8.GetBytes(csv.ToString());
        }
    }

    public class MockArbolService : IArbolService
    {
        private readonly List<Arbol> _arboles = new();
        private readonly Random _random = new();

        public MockArbolService()
        {
            // Datos de ejemplo
            _arboles.Add(new Arbol
            {
                Id = "1",
                Especie = "Cedro",
                Ubicacion = "Zona Norte",
                Altura = 2.5,
                Diametro = 15,
                FechaPlantacion = DateTime.Now.AddDays(-20),
                FechaUltimaMedicion = DateTime.Now.AddDays(-2),
                Estado = "Vivo",
                CampañaId = "1",
                AlumnoAsignado = "Juan Pérez"
            });
        }

        public async Task<Arbol> RegistrarArbolAsync(Arbol arbol)
        {
            arbol.Id = Guid.NewGuid().ToString();
            _arboles.Add(arbol);
            return await Task.FromResult(arbol);
        }

        public async Task<Arbol> GetArbolByIdAsync(string id)
        {
            return await Task.FromResult(_arboles.FirstOrDefault(a => a.Id == id));
        }

        public async Task<List<Arbol>> GetArbolesByCampañaAsync(string campañaId)
        {
            return await Task.FromResult(_arboles
                .Where(a => a.CampañaId == campañaId)
                .ToList());
        }

        public async Task<Arbol> UpdateArbolAsync(Arbol arbol)
        {
            var index = _arboles.FindIndex(a => a.Id == arbol.Id);
            if (index != -1)
            {
                _arboles[index] = arbol;
            }
            return await Task.FromResult(arbol);
        }

        public async Task<bool> DeleteArbolAsync(string id)
        {
            var arbol = _arboles.FirstOrDefault(a => a.Id == id);
            if (arbol != null)
            {
                _arboles.Remove(arbol);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> AsignarArbolAAlumnoAsync(string arbolId, string alumnoNombre)
        {
            var arbol = await GetArbolByIdAsync(arbolId);
            if (arbol != null)
            {
                arbol.AlumnoAsignado = alumnoNombre;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<List<Arbol>> GetArbolesByInspectorAsync(string inspectorId)
        {
            return await Task.FromResult(_arboles
                .Where(a => a.InspectorId == inspectorId)
                .ToList());
        }

        public async Task<Dictionary<string, int>> GetEstadisticasArbolesAsync(string campañaId)
        {
            var arboles = await GetArbolesByCampañaAsync(campañaId);
            var stats = new Dictionary<string, int>
            {
                ["Total"] = arboles.Count,
                ["Vivos"] = arboles.Count(a => a.Estado == "Vivo"),
                ["Enfermos"] = arboles.Count(a => a.Estado == "Enfermo"),
                ["Muertos"] = arboles.Count(a => a.Estado == "Muerto")
            };
            return stats;
        }

        public async Task<byte[]> ExportArbolesToCSVAsync(string campañaId)
        {
            var arboles = await GetArbolesByCampañaAsync(campañaId);
            var csv = new StringBuilder();
            csv.AppendLine("ID,Especie,Ubicación,Altura,Diametro,Fecha Plantación,Estado,Alumno Asignado");
            
            foreach (var a in arboles)
            {
                csv.AppendLine($"{a.Id},{a.Especie},{a.Ubicacion},{a.Altura},{a.Diametro},{a.FechaPlantacion:yyyy-MM-dd},{a.Estado},{a.AlumnoAsignado}");
            }

            return Encoding.UTF8.GetBytes(csv.ToString());
        }
    }

    public class MockInspectorService : IInspectorService
    {
        private readonly List<Inspector> _inspectores = new();

        public MockInspectorService()
        {
            // Datos de ejemplo
            _inspectores.Add(new Inspector
            {
                Id = "1",
                UsuarioId = "3",
                Nombre = "Inspector Test",
                Email = "inspector@ejemplo.com",
                Telefono = "555123456",
                Especialidad = "Botánica",
                CampañaId = "1",
                TenantId = "tenant1",
                IsActive = true
            });
        }

        public async Task<Inspector> RegistrarInspectorAsync(Inspector inspector)
        {
            inspector.Id = Guid.NewGuid().ToString();
            _inspectores.Add(inspector);
            return await Task.FromResult(inspector);
        }

        public async Task<Inspector> GetInspectorByIdAsync(string id)
        {
            return await Task.FromResult(_inspectores.FirstOrDefault(i => i.Id == id));
        }

        public async Task<List<Inspector>> GetInspectoresByCampañaAsync(string campañaId)
        {
            return await Task.FromResult(_inspectores
                .Where(i => i.CampañaId == campañaId)
                .ToList());
        }

        public async Task<Inspector> UpdateInspectorAsync(Inspector inspector)
        {
            var index = _inspectores.FindIndex(i => i.Id == inspector.Id);
            if (index != -1)
            {
                _inspectores[index] = inspector;
            }
            return await Task.FromResult(inspector);
        }

        public async Task<bool> DeleteInspectorAsync(string id)
        {
            var inspector = _inspectores.FirstOrDefault(i => i.Id == id);
            if (inspector != null)
            {
                inspector.IsActive = false;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> ElevarPrivilegiosInspectorAsync(string inspectorId)
        {
            var inspector = await GetInspectorByIdAsync(inspectorId);
            if (inspector != null)
            {
                // En un caso real, aquí se elevarían los permisos
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<Inspector> GetInspectorByUsuarioIdAsync(string usuarioId)
        {
            return await Task.FromResult(_inspectores
                .FirstOrDefault(i => i.UsuarioId == usuarioId));
        }

        public async Task<List<Inspector>> GetInspectoresByTenantAsync(string tenantId)
        {
            return await Task.FromResult(_inspectores
                .Where(i => i.TenantId == tenantId)
                .ToList());
        }
    }

    public class MockTenantService : ITenantService
    {
        private readonly List<Tenant> _tenants = new();

        public MockTenantService()
        {
            // Datos de ejemplo
            _tenants.Add(new Tenant
            {
                Id = "tenant1",
                Nombre = "Municipio Ejemplo",
                Dominio = "ejemplo.com",
                Plan = "Básico",
                IsActive = true,
                FechaCreacion = DateTime.Now.AddMonths(-6)
            });
        }

        public async Task<Tenant> CrearTenantAsync(Tenant tenant)
        {
            tenant.Id = Guid.NewGuid().ToString();
            tenant.FechaCreacion = DateTime.Now;
            _tenants.Add(tenant);
            return await Task.FromResult(tenant);
        }

        public async Task<Tenant> GetTenantByIdAsync(string id)
        {
            return await Task.FromResult(_tenants.FirstOrDefault(t => t.Id == id));
        }

        public async Task<List<Tenant>> GetAllTenantsAsync()
        {
            return await Task.FromResult(_tenants);
        }

        public async Task<Tenant> UpdateTenantAsync(Tenant tenant)
        {
            var index = _tenants.FindIndex(t => t.Id == tenant.Id);
            if (index != -1)
            {
                _tenants[index] = tenant;
            }
            return await Task.FromResult(tenant);
        }

        public async Task<bool> DeleteTenantAsync(string id)
        {
            var tenant = _tenants.FirstOrDefault(t => t.Id == id);
            if (tenant != null)
            {
                tenant.IsActive = false;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> AsignarCoordinadorAsync(string tenantId, string coordinadorId)
        {
            var tenant = await GetTenantByIdAsync(tenantId);
            if (tenant != null && !tenant.CoordinadoresIds.Contains(coordinadorId))
            {
                tenant.CoordinadoresIds.Add(coordinadorId);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> RemoverCoordinadorAsync(string tenantId, string coordinadorId)
        {
            var tenant = await GetTenantByIdAsync(tenantId);
            if (tenant != null && tenant.CoordinadoresIds.Contains(coordinadorId))
            {
                tenant.CoordinadoresIds.Remove(coordinadorId);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<List<Usuario>> GetCoordinadoresPorTenantAsync(string tenantId)
        {
            // Mock: retorna algunos coordinadores
            var coordinadores = new List<Usuario>
            {
                new Usuario { Id = "1", Email = "coordinador@ejemplo.com", Nombre = "Coordinador Test", Rol = "Coordinador", TenantId = tenantId }
            };
            return await Task.FromResult(coordinadores);
        }
    }
}