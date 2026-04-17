using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Core
{
    using BahiKitab.Models;
    using BahiKitab.Services;
    using Dapper;
    using System.Threading.Channels;

    public class WorkflowBackgroundWorker : BaseDataService
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
            Task.Run(StartDailyScheduler);
        }

        // This is what you call from your UI/ViewModels
        public void EnqueueWork(string eventKey, object data)
        {
            _taskQueue.Writer.TryWrite(new AppEvent { EventKey = eventKey, TargetData = data });
        }

        private async Task StartDailyScheduler()
        {
            while (true)
            {
                // 1. Calculate time until next run (e.g., 9:00 AM tomorrow)
                DateTime now = DateTime.Now;
                DateTime nextRun = now.Date.AddHours(11); // 9:00 AM today
                if (now > nextRun) nextRun = nextRun.AddDays(1); // If already past 9am, set for tomorrow

                double delay = (nextRun - now).TotalMilliseconds;
                await Task.Delay((int)delay);

                // 2. Run the check
                //await ProcessDateBasedEvents();

                await ProcessDormantCustomerEvents();
            }
        }

        /*private async Task ProcessDateBasedEvents()
        {
            using (var db = GetConnection())
            {
                // Find customers whose birthday is today (ignoring year)
                var birthdayQuery = "SELECT * FROM leads WHERE MONTH(DOB) = MONTH(CURDATE()) AND DAY(DOB) = DAY(CURDATE())";
                var birthdayList = await db.QueryAsync<Customer>(birthdayQuery);

                foreach (var person in birthdayList)
                {
                    // Enqueue to the engine we already built!
                    EnqueueWork("OnBirthday", person);
                }          
            }
        }*/

        private async Task ProcessDormantCustomerEvents()
        {
            using (var db = GetConnection())
            {
                // 1. Get the user-defined inactivity period from WorkflowRules
                var rule = await db.QueryFirstOrDefaultAsync<WorkflowRuleModel>(
                    "SELECT * FROM WorkflowRules WHERE EventKey = 'OnCustomerDormant' AND IsActive = 1");

                if (rule == null) return;

                // 2. Query customers whose last order was exactly X days ago
                // We join the Customers table with the Orders table to find the MAX(OrderDate)
                string query = $@"
            SELECT c.* FROM leads c
            JOIN (
                SELECT lead_id, MAX(create_time) as LastOrder 
                FROM lead_orders 
                GROUP BY lead_id
            ) o ON c.Id = o.lead_id
            WHERE DATEDIFF(CURDATE(), o.LastOrder) >= @Days";

                var dormantCustomers = await db.QueryAsync<Lead>(query, new { Days = rule.InactivityDays });

                foreach (var customer in dormantCustomers)
                {
                    // Reuse your existing logic!
                    EnqueueWork("OnCustomerDormant", customer);
                }
            }
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
