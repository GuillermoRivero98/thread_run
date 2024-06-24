using System;
using System.Threading.Tasks;

namespace SistemaSeguridad
{
    public class ControlAcceso
    {
        // Método para controlar el acceso y la captura de imágenes
        public async Task ControlarAccesoYCapturarImagen()
        {
            Console.WriteLine($"{DateTime.Now} [ControlAcceso] Controlando acceso...");

            // Simular control de acceso y otras operaciones
            await Task.Delay(2000); // Ejemplo: Simular tiempo de procesamiento

            // Preguntar al usuario si desea capturar una nueva imagen
            Console.Write("Nuevo rostro reconocido ¿Desea capturar esta nueva imagen? (S/N): ");
            string? respuesta = Console.ReadLine()?.ToUpper();

            if (respuesta == "S")
            {
                await CapturarNuevaImagen();
            }
            else if (respuesta == "N")
            {
                Console.WriteLine($"{DateTime.Now} [ControlAcceso] El administrador ha decidido no capturar una nueva imagen.");
            }
            else
            {
                Console.WriteLine($"{DateTime.Now} [ControlAcceso] Respuesta no reconocida. No se capturará una nueva imagen.");
            }

            Console.WriteLine($"{DateTime.Now} [ControlAcceso] Control de acceso terminado.");
        }

        // Método para capturar una nueva imagen
        private async Task CapturarNuevaImagen()
        {
            string nombreImagen = Guid.NewGuid().ToString(); // Generar nombre único
            Console.WriteLine($"{DateTime.Now} [ControlAcceso] Capturando nueva imagen: {nombreImagen}");

            // Aquí iría la lógica real para capturar la imagen y guardarla
            // Simulación de guardar imagen
            await Task.Delay(2000); // Ejemplo: Simular tiempo de captura y guardado

            Console.WriteLine($"{DateTime.Now} [ControlAcceso] Rostro identificado y guardado en: /ruta/de/imagen/{nombreImagen}.jpg");
        }
    }
}
