using BahiKitab.Core;
using BahiKitab.Models;
using BahiKitab.Services;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BahiKitab.ViewModels
{
    public class WorkflowViewModel : ViewModelBase
    {
        private readonly WorkflowService _service = new WorkflowService();

        // Collection for the DataGrid
        public ObservableCollection<WorkflowRuleModel> SavedRules { get; set; } = new();

        // Command to toggle status
        public ICommand ToggleStatusCommand { get; }

        // UI Bindings
        public ObservableCollection<EventType> EventList { get; set; } = new();

        private EventType _selectedEvent;
        public EventType SelectedEvent
        {
            get => _selectedEvent;
            set => Set(ref _selectedEvent, value, nameof(SelectedEvent));
        }

        public WorkflowRuleModel CurrentRule { get; set; } = new();

        public ICommand SaveCommand { get; }

        public WorkflowViewModel()
        {
            SaveCommand = new RelayCommand(async _  => await ExecuteSave());
            ToggleStatusCommand = new AsyncRelayCommand<WorkflowRuleModel>(async (rule) => await ExecuteToggle(rule));
            LoadData();
        }

        private async void LoadData()
        {
            var events = await _service.GetEventTypesAsync();
            foreach (var ev in events) EventList.Add(ev);
            // Load existing rules for the DataGrid
            await RefreshRulesList();
        }

        public async Task RefreshRulesList()
        {
            SavedRules.Clear();
            var rules = await _service.GetAllRulesAsync(); // Create this in WorkflowService
            foreach (var r in rules) SavedRules.Add(r);
        }

        private async Task ExecuteSave()
        {
            if (SelectedEvent == null) return;

            CurrentRule.EventKey = SelectedEvent.EventKey;
            await _service.SaveRuleAsync(CurrentRule);
            await RefreshRulesList();
            MessageBox.Show("Automation Saved Successfully!");
        }

        private async Task ExecuteToggle(WorkflowRuleModel rule)
        {
            if (rule == null) return;
            // The CheckBox is already bound to IsActive, so we just update the DB
            await _service.UpdateRuleStatusAsync(rule.Id, rule.IsActive);
        }
    }
}
