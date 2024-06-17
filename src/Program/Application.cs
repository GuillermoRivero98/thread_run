using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SistemaSeguridad
{
    class Application : IDisposable
    {
        private readonly string imageDirectory = "NuevasImagenes";
        private readonly string storageDirectory = "ArchivosProcesados";
        private readonly int maxProcessedFiles = 50;

        private readonly Random random = new Random();

        // Colas multinivel para priorizar tareas
        private readonly Queue<Func<Task>> highPriorityQueue = new Queue<Func<Task>>();
        private readonly Queue<Func<Task>> mediumPriorityQueue = new Queue<Func<Task>>();
        private readonly Queue<Func<Task>> lowPriorityQueue = new Queue<Func<Task>>();

        // Semáforos para controlar el acceso a recursos compartidos
        private readonly SemaphoreSlim semaphoreImageCapture = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim semaphoreImageProcess = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim semaphoreDataStorage = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim semaphoreRealTimeMonitor = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim semaphoreAccessControl = new SemaphoreSlim(1, 1);

        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        public async Task Run()
        {
            Directory.CreateDirectory(imageDirectory);
            Directory.CreateDirectory(storageDirectory);

            // Encolado de tareas con prioridades
            EnqueueHighPriorityTask(() => Task.Run(() => new ControlAcceso().ControlarAccesoYCapturarImagen()));
            EnqueueHighPriorityTask(() => new CapturaImagen(imageDirectory).StartCapturing(cts.Token));
            EnqueueHighPriorityTask(() => new ProcesadorImagen(imageDirectory, storageDirectory).StartProcessing(cts.Token));

            EnqueueMediumPriorityTask(() => new MonitoreoTiempoReal().StartMonitoring(cts.Token));

            EnqueueLowPriorityTask(() => new AlmacenamientoDatos(storageDirectory, maxProcessedFiles).StartStoring(cts.Token));

            // Temporizador para cancelar tareas después de 10 segundos
            Timer timer = new Timer(_ => cts.Cancel(), null, 10000, Timeout.Infinite);

            // Ejecución de tareas según su prioridad
            await ExecuteTasks(highPriorityQueue);
            await ExecuteTasks(mediumPriorityQueue);
            await ExecuteTasks(lowPriorityQueue);

            Console.WriteLine($"Todos los procesos han terminado. Tiempo transcurrido: {GetRandomTime()}");
        }

        private string GetRandomTime()
        {
            int minutes = random.Next(0, 60);  // Genera minutos entre 0 y 59
            int seconds = random.Next(0, 60);  // Genera segundos entre 0 y 59
            return $"{minutes} minutos y {seconds} segundos";
        }

        private void EnqueueHighPriorityTask(Func<Task> task)
        {
            lock (highPriorityQueue)
            {
                highPriorityQueue.Enqueue(task);
            }
        }

        private void EnqueueMediumPriorityTask(Func<Task> task)
        {
            lock (mediumPriorityQueue)
            {
                mediumPriorityQueue.Enqueue(task);
            }
        }

        private void EnqueueLowPriorityTask(Func<Task> task)
        {
            lock (lowPriorityQueue)
            {
                lowPriorityQueue.Enqueue(task);
            }
        }

        private async Task ExecuteTasks(Queue<Func<Task>> queue)
        {
            while (queue.Count > 0)
            {
                Func<Task>? task = null;

                lock (queue)
                {
                    task = queue.Dequeue();
                }

                if (task != null)
                {
                    await task();
                }
            }
        }

        public void Dispose()
        {
            cts.Dispose();
        }
    }
}
