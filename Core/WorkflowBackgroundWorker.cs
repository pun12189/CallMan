using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Core
{
    using BahiKitab.Models;
    using System.Threading.Channels;

    public class WorkflowBackgroundWorker
    {
        private readonly Channel<AppEvent> _taskQueue;
        private readonly WorkflowEngine _engine;

        public WorkflowBackgroundWorker()
        {
            // Unbounded means it can hold as many tasks as memory allows
            _taskQueue = Channel.CreateUnbounded<AppEvent>();
            _engine = new WorkflowEngine();

            // Start the consumer thread immediately
            Task.Run(ProcessQueueAsync);
        }

        // This is what you call from your UI/ViewModels
        public void EnqueueWork(string eventKey, object data)
        {
            _taskQueue.Writer.TryWrite(new AppEvent { EventKey = eventKey, TargetData = data });
        }

        private async Task ProcessQueueAsync()
        {
            // This loop runs forever in the background
            await foreach (var appEvent in _taskQueue.Reader.ReadAllAsync())
            {
                try
                {
                    // Execute the logic we wrote previously
                    await _engine.TriggerEventAsync(appEvent.EventKey, appEvent.TargetData);
                }
                catch (Exception ex)
                {
                    // Log error so the background thread doesn't die
                    System.Diagnostics.Debug.WriteLine($"Background Worker Error: {ex.Message}");
                }
            }
        }
    }
}
