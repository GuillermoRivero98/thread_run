using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaSeguridad
{
    public class Application : IDisposable
    {
        private readonly SimulationConfig config;
        private readonly string imageDirectory = "imagenes";
        private readonly string storageDirectory = "almacenamiento";
        private readonly int maxProcessedFiles = 100;
        private readonly SemaphoreSlim semaphoreImageCapture = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim semaphoreRealTimeMonitor = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim semaphoreDataStorage = new SemaphoreSlim(1, 1);
        private readonly Queue<Func<Task>> highPriorityQueue = new Queue<Func<Task>>();
        private readonly Queue<Func<Task>> mediumPriorityQueue = new Queue<Func<Task>>();
        private readonly Queue<Func<Task>> lowPriorityQueue = new Queue<Func<Task>>();
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
            EnqueueMediumPriorityTask(
                () =>
                    new Monitoreo().IniciarMonitoreo(cts.Token)  // Aquí se usa la clase Monitoreo
            );
            EnqueueLowPriorityTask(
                () =>
                    new AlmacenamientoDatos(storageDirectory, maxProcessedFiles).IniciarAlmacenamiento(cts.Token)
            );

            var highPriorityTask = ProcessQueueAsync(highPriorityQueue, semaphoreImageCapture, cts.Token);
            var mediumPriorityTask = ProcessQueueAsync(mediumPriorityQueue, semaphoreRealTimeMonitor, cts.Token);
            var lowPriorityTask = ProcessQueueAsync(lowPriorityQueue, semaphoreDataStorage, cts.Token);

            await Task.WhenAll(highPriorityTask, mediumPriorityTask, lowPriorityTask);
        }

        private void EnqueueHighPriorityTask(Func<Task> task)
        {
            highPriorityQueue.Enqueue(task);
        }

        private void EnqueueMediumPriorityTask(Func<Task> task)
        {
            mediumPriorityQueue.Enqueue(task);
        }

        private void EnqueueLowPriorityTask(Func<Task> task)
        {
            lowPriorityQueue.Enqueue(task);
        }

        private async Task ProcessQueueAsync(Queue<Func<Task>> queue, SemaphoreSlim semaphore, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Func<Task> task;

                lock (queue)
                {
                    if (queue.Count == 0)
                        break;

                    task = queue.Dequeue();
                }

                await semaphore.WaitAsync(token);
                try
                {
                    await task();
                }
                finally
                {
                    semaphore.Release();
                }
            }
        }

        public void Dispose()
        {
            cts.Cancel();
            cts.Dispose();
            semaphoreImageCapture.Dispose();
            semaphoreRealTimeMonitor.Dispose();
            semaphoreDataStorage.Dispose();
        }
    }
}
