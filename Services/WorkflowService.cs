using BahiKitab.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Services
{
    public class WorkflowService : BaseDataService
    {
        // 1. Fetch all available event types for the ComboBox
        public async Task<IEnumerable<EventType>> GetEventTypesAsync()
        {
            using IDbConnection db = GetConnection();
            return await db.QueryAsync<EventType>("SELECT * FROM EventRegistry");
        }

        // 2. Save or Update a User-Defined Rule
        public async Task SaveRuleAsync(WorkflowRuleModel rule)
        {
            using IDbConnection db = GetConnection();
            string sql = @"INSERT INTO WorkflowRules (EventKey, IsActive, SendWhatsApp, SendEmail, MessageTemplate) 
                       VALUES (@EventKey, @IsActive, @SendWhatsApp, @SendEmail, @MessageTemplate, @InactivityDays)
                       ON DUPLICATE KEY UPDATE 
                       IsActive = @IsActive, SendWhatsApp = @SendWhatsApp, 
                       SendEmail = @SendEmail, MessageTemplate = @MessageTemplate,
                   InactivityDays = @InactivityDays";

            await db.ExecuteAsync(sql, rule);
        }

        // 3. Get Active Rules for a specific event (Used by the Engine)
        public async Task<IEnumerable<WorkflowRuleModel>> GetActiveRulesForEventAsync(string eventKey)
        {
            using IDbConnection db = GetConnection();
            return await db.QueryAsync<WorkflowRuleModel>(
                "SELECT * FROM WorkflowRules WHERE EventKey = @EventKey AND IsActive = 1",
                new { EventKey = eventKey });
        }

        public async Task<IEnumerable<WorkflowRuleModel>> GetAllRulesAsync()
        {
            using IDbConnection db = GetConnection();
            return await db.QueryAsync<WorkflowRuleModel>(@"
        SELECT r.*, e.DisplayName 
        FROM WorkflowRules r 
        JOIN EventRegistry e ON r.EventKey = e.EventKey");
        }

        public async Task UpdateRuleStatusAsync(int id, bool isActive)
        {
            using IDbConnection db = GetConnection();
            await db.ExecuteAsync("UPDATE WorkflowRules SET IsActive = @isActive WHERE Id = @id", new { isActive, id });
        }
    }
}
