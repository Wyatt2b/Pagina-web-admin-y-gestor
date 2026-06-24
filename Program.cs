using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AdminArboles.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Registrar servicios
builder.Services.AddScoped<IAuthService, MockAuthService>();
builder.Services.AddScoped<ICampañaService, MockCampañaService>();
builder.Services.AddScoped<IArbolService, MockArbolService>();
builder.Services.AddScoped<IInspectorService, MockInspectorService>();
builder.Services.AddScoped<ITenantService, MockTenantService>();
builder.Services.AddScoped<IDownloadService, DownloadService>(); // 👈 Agregar

// HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Autenticación y autorización
builder.Services.AddAuthorizationCore();
builder.Services.AddAuthenticationCore();

await builder.Build().RunAsync();