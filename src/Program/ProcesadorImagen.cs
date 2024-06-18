using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace SistemaSeguridad
{
namespace SistemaSeguridad
{
    public class ProcesadorImagen
    {
        private readonly string _imageDirectory;
        private readonly string _storageDirectory;
        private readonly int _networkLatency;
        private static Random random = new Random();

        public ProcesadorImagen(string imageDirectory, string storageDirectory, int networkLatency)
        {
            _imageDirectory = imageDirectory;
            _storageDirectory = storageDirectory;
            _networkLatency = networkLatency;
            Directory.CreateDirectory(_storageDirectory); 
        }

        public async Task StartProcessing(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                string[] imagePaths = Directory.GetFiles(_imageDirectory);

                foreach (string imagePath in imagePaths)
                {
                    Log($"Procesando imagen: {Path.GetFileNameWithoutExtension(imagePath)}");

                    // Simular latencia de red
                    await Task.Delay(_networkLatency);

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

}
