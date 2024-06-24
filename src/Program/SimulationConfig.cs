using System;

namespace SistemaSeguridad
{
    public class SimulationConfig
    {
        public int NumCameras { get; set; }
        public (int width, int height) ImageResolution { get; set; }
        public int CaptureFrequency { get; set; }
        public string TrafficLevel { get; set; }
        public int CPUAllocation { get; set; }
        public int RAMAllocation { get; set; }
        public int NetworkLatency { get; set; }
        public int NetworkBandwidth { get; set; }

        public SimulationConfig()
        {
            // Valores por defecto
            NumCameras = 5;
            ImageResolution = (1920, 1080);
            CaptureFrequency = 1;
            TrafficLevel = "medium";
            CPUAllocation = 4;
            RAMAllocation = 8;
            NetworkLatency = 50;
            NetworkBandwidth = 100;
        }

        public void DisplayConfig()
        {
            Console.WriteLine($"Configuración de Simulación:\n" +
                              $"Número de Cámaras: {NumCameras}\n" +
                              $"Resolución de Imagen: {ImageResolution}\n" +
                              $"Frecuencia de Captura: {CaptureFrequency} segundos\n" +
                              $"Nivel de Tráfico: {TrafficLevel}\n" +
                              $"Asignación de CPU: {CPUAllocation} núcleos\n" +
                              $"Asignación de RAM: {RAMAllocation} GB\n" +
                              $"Latencia de Red: {NetworkLatency} ms\n" +
                              $"Ancho de Banda de Red: {NetworkBandwidth} Mbps\n");
        }
    }
}
