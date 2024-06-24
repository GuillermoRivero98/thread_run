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
        private readonly int _networkLatency;

        public ProcesadorImagen(string imageDirectory, string storageDirectory, int networkLatency)
        {
            _imageDirectory = imageDirectory;
            _storageDirectory = storageDirectory;
            _networkLatency = networkLatency;
        }

        public async Task StartProcessing(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                string[] files = Directory.GetFiles(_imageDirectory);
                foreach (var file in files)
                {
                    // Simulación de procesamiento de imagen
                    await Task.Delay(_networkLatency);
                    string outputFileName = Path.Combine(_storageDirectory, Path.GetFileName(file) + ".txt");
                    await File.WriteAllTextAsync(outputFileName, "Datos procesados de la imagen");
                    Log($"Imagen procesada: {file}, datos almacenados en: {outputFileName}");
                    File.Delete(file);
                }
                await Task.Delay(1000); // Simular tiempo de espera para el próximo procesamiento
            }
        }

        private void Log(string mensaje)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [ProcesadorImagen] {mensaje}");
        }
    }
}
