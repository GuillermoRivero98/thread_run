using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaSeguridad
{
    public class ProcesadorImagen
    {
        private readonly string _imageDirectory;
        private readonly string _storageDirectory;
        private static Random random = new Random();

        public ProcesadorImagen(string imageDirectory, string storageDirectory)
        {
            _imageDirectory = imageDirectory;
            _storageDirectory = storageDirectory;
            Directory.CreateDirectory(_storageDirectory);  // Asegurarnos de que el directorio de almacenamiento exista
        }

        public async Task StartProcessing(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                string[] imagePaths = Directory.GetFiles(_imageDirectory);

                foreach (string imagePath in imagePaths)
                {
                    Log($"Procesando imagen: {Path.GetFileNameWithoutExtension(imagePath)}");

                    // Mover la imagen al directorio de almacenamiento
                    string processedPath = Path.Combine(_storageDirectory, Path.GetFileName(imagePath));
                    File.Move(imagePath, processedPath);
                    Log($"Imagen movida a: {processedPath}");

                    await Task.Delay(random.Next(1000, 3000));
                }

                await Task.Delay(3000);
            }
        }

        private void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [Procesamiento] {message}");
        }
    }
}
