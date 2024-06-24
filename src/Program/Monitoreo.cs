using System;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaSeguridad
{
    public class Monitoreo
    {
        private static Random random = new Random();

        public async Task IniciarMonitoreo(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Log("Monitoreando en tiempo real...");

                // Simulación de eventos en tiempo real
                if (random.NextDouble() < 0.1)
                {
                    Log("Alerta: ¡Evento sospechoso detectado!");
                }

                await Task.Delay(random.Next(2300, 2700)); // Simular tiempo de monitoreo
            }

            Log("Monitoreo en tiempo real terminado.");
        }

        private void Log(string mensaje)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [Monitoreo] {mensaje}");
        }
    }
}
