using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaSeguridad
{
    public class CapturaImagen
    {
        private readonly string _imageDirectory;
        private readonly (int width, int height) _imageResolution;
        private readonly int _captureFrequency;

        public CapturaImagen(string imageDirectory, (int width, int height) imageResolution, int captureFrequency)
        {
            _imageDirectory = imageDirectory;
            _imageResolution = imageResolution;
            _captureFrequency = captureFrequency;
        }

        public async Task StartCapturing(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                string fileName = Path.Combine(_imageDirectory, $"captura_{DateTime.Now:yyyyMMddHHmmss}.jpg");
                // Simulación de captura de imagen
                await File.WriteAllTextAsync(fileName, "Imagen simulada");
                Log($"Imagen capturada y almacenada en: {fileName}");
                await Task.Delay(_captureFrequency * 1000);
            }
        }

        private void Log(string mensaje)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [CapturaImagen] {mensaje}");
        }
    }
}
