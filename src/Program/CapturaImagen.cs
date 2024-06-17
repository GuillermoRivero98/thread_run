using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaSeguridad
{
    public class CapturaImagen
    {
        private readonly string _imageDirectory;
        private static Random random = new Random();

        public CapturaImagen(string imageDirectory)
        {
            _imageDirectory = imageDirectory;
            Directory.CreateDirectory(_imageDirectory);  // Asegurarnos de que el directorio exista
        }

        public async Task StartCapturing(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (await UserWantsToCaptureImageAsync(token))
                {
                    string imageId = Guid.NewGuid().ToString();
                    string imagePath = Path.Combine(_imageDirectory, $"imagen_{imageId}.jpg");

                    if (!File.Exists(imagePath))
                    {
                        Log($"Capturando nueva imagen: {imageId}");
                        File.WriteAllText(imagePath, $"Contenido de la imagen con número aleatorio: {random.Next(1000)}");
                        Log($"Imagen guardada en: {imagePath}");
                    }
                    else
                    {
                        Log($"La imagen {imageId} ya existe. La captura ha sido denegada.");
                    }
                }
                else
                {
                    Log("El administrador ha decidido no capturar una nueva imagen.");
                }

                await Task.Delay(random.Next(500, 2000));
            }
        }

        private async Task<bool> UserWantsToCaptureImageAsync(CancellationToken token)
        {
            Console.WriteLine("¿Desea capturar una nueva imagen? (S/N)");
            string response = (await Task.Run(() => Console.ReadLine(), token))?.Trim().ToUpper() ?? string.Empty;
            return response == "S";
        }

        private void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [Captura] {message}");
        }
    }
}
