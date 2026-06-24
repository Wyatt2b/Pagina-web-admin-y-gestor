// Services/DownloadService.cs
using Microsoft.JSInterop;

namespace AdminArboles.Services
{
    public class DownloadService : IDownloadService
    {
        private readonly IJSRuntime _jsRuntime;

        public DownloadService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task DownloadFileAsync(byte[] data, string fileName, string contentType)
        {
            // Convertir a base64
            var base64 = Convert.ToBase64String(data);
            
            // Crear el script JavaScript inline
            var jsScript = $@"
                (function() {{
                    // Decodificar base64
                    const byteCharacters = atob('{base64}');
                    const byteNumbers = new Array(byteCharacters.length);
                    for (let i = 0; i < byteCharacters.length; i++) {{
                        byteNumbers[i] = byteCharacters.charCodeAt(i);
                    }}
                    const byteArray = new Uint8Array(byteNumbers);
                    
                    // Crear blob y enlace de descarga
                    const blob = new Blob([byteArray], {{ type: '{contentType}' }});
                    const url = window.URL.createObjectURL(blob);
                    const link = document.createElement('a');
                    link.href = url;
                    link.download = '{fileName}';
                    document.body.appendChild(link);
                    link.click();
                    document.body.removeChild(link);
                    window.URL.revokeObjectURL(url);
                }})();
            ";

            await _jsRuntime.InvokeVoidAsync("eval", jsScript);
        }
    }
}