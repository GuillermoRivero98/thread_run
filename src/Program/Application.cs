using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaSeguridad
{
    class Application : IDisposable
    {
        private readonly string imageDirectory = "NuevasImagenes";
        private readonly string storageDirectory = "ArchivosProcesados";
        private readonly int maxProcessedFiles = 50;
        private readonly SimulationConfig config;
        private readonly Random random = new Random();

        private readonly Queue<Func<Task>> highPriorityQueue = new Queue<Func<Task>>();
        private readonly Queue<Func<Task>> mediumPriorityQueue = new Queue<Func<Task>>();
        private readonly Queue<Func<Task>> lowPriorityQueue = new Queue<Func<Task>>();

        private readonly SemaphoreSlim semaphoreImageCapture = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim semaphoreImageProcess = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim semaphoreDataStorage = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim semaphoreRealTimeMonitor = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim semaphoreAccessControl = new SemaphoreSlim(1, 1);

        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        public Application(SimulationConfig config)
        {
            this.config = config;
        }

        public async Task Run()
        {
            config.DisplayConfig();
            Directory.CreateDirectory(imageDirectory);
            Directory.CreateDirectory(storageDirectory);

            EnqueueHighPriorityTask(
                () => Task.Run(() => new ControlAcceso().ControlarAccesoYCapturarImagen())
            );
            EnqueueHighPriorityTask(
                () =>
                    new CapturaImagen(
                        imageDirectory,
                        config.ImageResolution,
                        config.CaptureFrequency
                    ).StartCapturing(cts.Token)
            );
            EnqueueHighPriorityTask(
                () =>
                    new ProcesadorImagen(
                        imageDirectory,
                        storageDirectory,
                        config.NetworkLatency
                    ).StartProcessing(cts.Token)
            );

            EnqueueMediumPriorityTask(() => new MonitoreoTiempoReal().StartMonitoring(cts.Token));

            EnqueueLowPriorityTask(
                () =>
                    new AlmacenamientoDatos(storageDirectory, maxProcessedFiles).StartStoring(
                        cts.Token
                    )
            );

            Timer timer = new Timer(_ => cts.Cancel(), null, 10000, Timeout.Infinite);

            await ExecuteTasks(highPriorityQueue);
            await ExecuteTasks(mediumPriorityQueue);
            await ExecuteTasks(lowPriorityQueue);

            Console.WriteLine(
                $"Todos los procesos han terminado. Tiempo transcurrido: {GetRandomTime()}"
            );
        }

        private string GetRandomTime()
        {
            int minutes = random.Next(0, 60);
            int seconds = random.Next(0, 60);
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
