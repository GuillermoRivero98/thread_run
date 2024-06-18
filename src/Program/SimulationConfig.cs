using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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
            Console.WriteLine($"Simulation Configuration:\n" +
                              $"Number of Cameras: {NumCameras}\n" +
                              $"Image Resolution: {ImageResolution}\n" +
                              $"Capture Frequency: {CaptureFrequency} seconds\n" +
                              $"Traffic Level: {TrafficLevel}\n" +
                              $"CPU Allocation: {CPUAllocation} cores\n" +
                              $"RAM Allocation: {RAMAllocation} GB\n" +
                              $"Network Latency: {NetworkLatency} ms\n" +
                              $"Network Bandwidth: {NetworkBandwidth} Mbps\n");
        }
    }
}
