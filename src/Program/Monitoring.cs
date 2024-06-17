using System;
using System.Threading;
using System.Threading.Tasks;

namespace SecuritySystem
{
    public class Monitoring
    {
        public async Task StartMonitoring(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Log("Monitoring in real-time...");
                await Task.Delay(2500); // Simular tiempo de monitoreo
            }

            Log("Real-time monitoring finished.");
        }

        private void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [Monitoring] {message}");
        }
    }
}
