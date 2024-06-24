using System;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaSeguridad
{
    public class MonitoreoTiempoReal
    {
        private static Random random = new Random();

        public async Task StartMonitoring(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Log("Monitoreando en tiempo real...");

                // Simulación de eventos en tiempo real
                if (random.NextDouble() < 0.1)
                {
                    Log("Alerta: Evento sospechoso detectado!");
                }

                await Task.Delay(random.Next(2300, 2700));
            }

            Log("Monitoreo en tiempo real terminado.");
        }

        private void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [Monitoreo] {message}");
        }
    }
}
