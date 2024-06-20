using System;
using System.Threading.Tasks;

namespace SistemaSeguridad
{
    class Program
    {
        static async Task Main()
        {
            var config = new SimulationConfig
            {
                NumCameras = 10,
                ImageResolution = (1280, 720),
                CaptureFrequency = 2,
                TrafficLevel = "high",
                CPUAllocation = 8,
                RAMAllocation = 16,
                NetworkLatency = 20,
                NetworkBandwidth = 200
            };

            var app = new Application(config);
            await app.Run();
        }
    }
}
