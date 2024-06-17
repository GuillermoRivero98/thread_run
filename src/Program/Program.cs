using System;
using System.Threading.Tasks;

namespace SistemaSeguridad
{
    class Program
    {
        static async Task Main()
        {
            // Inicializar la aplicación
            var app = new Application();

            // Iniciar la ejecución de la aplicación
            await app.Run();
        }
    }
}
