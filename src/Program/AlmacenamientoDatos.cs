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

        public async Task StartStoring(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                string[] processedFiles = Directory.GetFiles(_storageDirectory);

                if (processedFiles.Length > _maxProcessedFiles)
                {
                    foreach (string filePath in processedFiles)
                    {
                        Log($"Almacenando archivo: {Path.GetFileName(filePath)}");

                        // Simular la operación de almacenamiento
                        await Task.Delay(random.Next(1000, 2000));

                        // Eliminar el archivo después de "almacenarlo"
                        File.Delete(filePath);
                        Log($"Archivo almacenado y eliminado: {filePath}");

                        if (!token.IsCancellationRequested)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    Log("No hay suficientes archivos procesados para almacenar.");
                }

                await Task.Delay(5000);  // Esperar antes de la siguiente verificación
            }

            Log("Proceso de almacenamiento de datos terminado.");
        }

        private void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [Almacenamiento] {message}");
        }
    }
}
