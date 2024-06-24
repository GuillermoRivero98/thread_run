using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaSeguridad
{
    public class ProcesadorImagen
    {
        private readonly string _inputDirectory;
        private readonly string _outputDirectory;
        private readonly int _networkLatency;
        private static Random random = new Random();

        public ProcesadorImagen(string inputDirectory, string outputDirectory, int networkLatency)
        {
            _inputDirectory = inputDirectory;
            _outputDirectory = outputDirectory;
            _networkLatency = networkLatency;
            Directory.CreateDirectory(_outputDirectory);  // Asegurarnos de que el directorio de salida exista
        }

        public async Task StartProcessing(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                string[] inputFiles = Directory.GetFiles(_inputDirectory, "*.jpg");

                if (inputFiles.Length > 0)
                {
                    string inputFile = inputFiles[random.Next(inputFiles.Length)];
                    string outputFile = Path.Combine(_outputDirectory, Path.GetFileName(inputFile) + ".txt");

                    Log($"Procesando imagen: {inputFile}");
                    await Task.Delay(_networkLatency); // Simular latencia de red

                    // Simulación de procesamiento de la imagen
                    await File.WriteAllTextAsync(outputFile, $"Procesada: {inputFile}");

                    Log($"Imagen procesada y guardada en: {outputFile}");
                    File.Delete(inputFile); // Eliminar la imagen original después de procesarla
                }

                await Task.Delay(random.Next(2300, 2700));
            }

            Log("Procesamiento de imágenes terminado.");
        }

        private void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [Procesador] {message}");
        }
    }
}
