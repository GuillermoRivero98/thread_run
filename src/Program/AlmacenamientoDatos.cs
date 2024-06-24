using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaSeguridad
{
    public class AlmacenamientoDatos
    {
        private readonly string _storageDirectory;
        private readonly int _maxProcessedFiles;
        private static Random random = new Random();

        public AlmacenamientoDatos(string storageDirectory, int maxProcessedFiles)
        {
            _storageDirectory = storageDirectory;
            _maxProcessedFiles = maxProcessedFiles;
            Directory.CreateDirectory(_storageDirectory);  // Asegurarnos de que el directorio de almacenamiento exista
        }

        public async Task IniciarAlmacenamiento(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                string[] processedFiles = Directory.GetFiles(_storageDirectory);

                if (processedFiles.Length >= _maxProcessedFiles)
                {
                    Array.Sort(processedFiles, (a, b) => File.GetCreationTime(a).CompareTo(File.GetCreationTime(b)));
                    File.Delete(processedFiles[0]);
                    Log($"Archivo más antiguo eliminado. Nuevo tamaño de almacenamiento: {processedFiles.Length - 1}");
                }

                string outputFilePath = Path.Combine(_storageDirectory, $"procesada_{DateTime.Now:yyyyMMddHHmmss}.jpg.txt");
                string imageData = "Datos procesados de la imagen";
                await File.WriteAllTextAsync(outputFilePath, imageData);
                Log($"Datos almacenados en: {outputFilePath}");

                await Task.Delay(random.Next(2800, 3200));
            }

            Log("Almacenamiento de datos terminado.");
        }

        private void Log(string mensaje)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [Almacenamiento] {mensaje}");
        }
    }
}
