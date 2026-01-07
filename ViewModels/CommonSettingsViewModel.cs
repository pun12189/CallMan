using BahiKitab.Core;
using BahiKitab.Helper;
using BahiKitab.Models;
using BahiKitab.Services;
using BahiKitab.Views;
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
    public class CommonSettingsViewModel : ViewModelBase
    {
        // Services
        private readonly DeadReasonsDataService _deadReasonDataService;
        private readonly MatureStagesDataService _matureStagesDataService;
        private readonly FollowUpStagesDataService _followUpStagesDataService;
        private readonly LeadLabelsDataService _leadLabelsDataService;
        private readonly LeadSourceDataService _leadSourceDataService;
        private readonly LeadTagsDataService _leadTagsDataService;
        private readonly OrderStagesDataService _orderStagesDataService;
        private readonly DepartmentsDataService _departmentsDataService;
        private readonly LeadStatusDataService _leadStatusDataService;
        private string textBoxValue;

        // Dynamic Views Command call from Admin View Model

        public ICommand DynamicViewCommand { get; set; }

        public ICommand DynamicLoadViewCommand { get; set; }

        public ICommand DynamicAddViewCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        // Constructor

        public CommonSettingsViewModel()
        {
            this.DynamicViewCommand = new RelayCommand(LoadDynamicView);
            this.DynamicLoadViewCommand = new AsyncRelayCommand<object>(LoadDynamicData, CanSaveLead);
            this.DynamicAddViewCommand = new AsyncRelayCommand<object>(AddButtonCommandExecute, CanSaveLead);

            this.DeleteCommand = new AsyncRelayCommand<object>(DynamicDeleteCommandExecute, CanSaveLead);

            this.RefreshCommand = new AsyncRelayCommand<object>(LoadDynamicData, CanSaveLead);

            this._deadReasonDataService = new DeadReasonsDataService();
            this._matureStagesDataService = new MatureStagesDataService();
            this._followUpStagesDataService = new FollowUpStagesDataService();
            this._leadLabelsDataService = new LeadLabelsDataService();
            this._leadSourceDataService = new LeadSourceDataService();
            this._leadTagsDataService = new LeadTagsDataService();
            this._departmentsDataService = new DepartmentsDataService();
            this._orderStagesDataService = new OrderStagesDataService();
            this._leadStatusDataService = new LeadStatusDataService();
        }

        private async Task DynamicDeleteCommandExecute(object arg)
        {
            var viewName = (ViewsEnum)arg;
            if (SelectedData == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete data?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                switch (viewName)
                {
                    case ViewsEnum.DeadReason:
                        var deadReasons = this.SelectedData as DeadReasonModel;
                        if (deadReasons != null)
                        {
                            await this._deadReasonDataService.DeleteDeadReasonAsync(deadReasons);
                            this.Data.Remove(deadReasons);
                        }

                        break;
                    case ViewsEnum.MatureStages:
                        var matureStages = this.SelectedData as MatureStagesModel;
                        if (matureStages != null)
                        {
                            await this._matureStagesDataService.DeleteMatureStagesAsync(matureStages);
                            this.Data.Remove(matureStages);
                        }

                        break;
                    case ViewsEnum.LeadSource:
                        var leadSources = this.SelectedData as LeadSourceModel;
                        if (leadSources != null)
                        {
                            await this._leadSourceDataService.DeleteLeadSourceAsync(leadSources);
                            this.Data.Remove(leadSources);
                        }

                        break;
                    case ViewsEnum.LeadLabels:
                        var leadLabels = this.SelectedData as LeadLabelsModel;
                        if (leadLabels != null)
                        {
                            await this._leadLabelsDataService.DeleteLeadLabelsAsync(leadLabels);
                            this.Data.Remove(leadLabels);
                        }

                        break;
                    case ViewsEnum.LeadTags:
                        var leadTags = this.SelectedData as LeadTagModel;
                        if (leadTags != null)
                        {
                            await this._leadTagsDataService.DeleteLeadTagAsync(leadTags);
                            this.Data.Remove(leadTags);
                        }

                        break;
                    case ViewsEnum.FollowUpStages:
                        var followUpStages = this.SelectedData as FollowUpStagesModel;
                        if (followUpStages != null)
                        {
                            await this._followUpStagesDataService.DeleteFollowUpStagesAsync(followUpStages);
                            this.Data.Remove(followUpStages);
                        }

                        break;
                    case ViewsEnum.Departments:
                        var departments = this.SelectedData as DepartmentsModel;
                        if (departments != null)
                        {
                            await this._departmentsDataService.DeleteDepartmentsAsync(departments);
                            this.Data.Remove(departments);
                        }

                        break;
                    case ViewsEnum.OrderStages:
                        var orderStages = this.SelectedData as OrderStageModel;
                        if (orderStages != null)
                        {
                            await this._orderStagesDataService.DeleteOrderStageAsync(orderStages);
                            this.Data.Remove(orderStages);
                        }

                        break;
                    case ViewsEnum.LeadStatus:
                        var leadStatus = this.SelectedData as LeadStatusModel;
                        if (leadStatus != null)
                        {
                            await this._leadStatusDataService.DeleteLeadStatusAsync(leadStatus);
                            this.Data.Remove(leadStatus);
                        }

                        break;
                    default:
                        MessageBox.Show("No view selected");
                        break;
                }
            }

            this.TextBoxValue = string.Empty;
        }

        private async Task LoadDynamicData(object arg)
        {
            var viewName = (ViewsEnum)arg;
            this.Data.Clear();
            switch (viewName)
            {
                case ViewsEnum.DeadReason:
                    var deadReasons = await this._deadReasonDataService.GetAllDeadReasonsAsync();
                    if (deadReasons != null)
                    {
                        foreach (var item in deadReasons)
                        {
                            this.Data.Add(item);
                        }
                    }                    
                    
                    break;
                case ViewsEnum.MatureStages:
                    var matureStages = await this._matureStagesDataService.GetAllMatureStagessAsync();
                    if (matureStages != null)
                    foreach (var item in matureStages)
                    {
                        this.Data.Add(item);
                    }

                    break;
                case ViewsEnum.LeadSource:
                    var leadSources = await this._leadSourceDataService.GetAllLeadSourcesAsync();
                    if (leadSources != null)
                    foreach (var item in leadSources)
                    {
                        this.Data.Add(item);
                    }
                    
                    break;
                case ViewsEnum.LeadLabels:
                    var leadLabels = await this._leadLabelsDataService.GetAllLeadLabelsAsync();
                    if (leadLabels != null)
                    foreach (var item in leadLabels)
                    {
                        this.Data.Add(item);
                    }

                    break;
                case ViewsEnum.LeadTags:
                    var leadTags = await this._leadTagsDataService.GetAllLeadTagsAsync();
                    if (leadTags != null)
                    foreach (var item in leadTags)
                    {
                        this.Data.Add(item);
                    }

                    break;
                case ViewsEnum.FollowUpStages:
                    var followUpStages = await this._followUpStagesDataService.GetAllFollowUpStagessAsync();
                    if (followUpStages != null)
                    foreach (var item in followUpStages)
                    {
                        this.Data.Add(item);
                    }

                    break;
                case ViewsEnum.Departments:
                    var departments = await this._departmentsDataService.GetAllDepartmentsAsync();
                    if (departments != null)
                    foreach (var item in departments)
                    {
                        this.Data.Add(item);
                    }

                    break;
                case ViewsEnum.OrderStages:
                    var orderStages = await this._orderStagesDataService.GetAllOrderStagesAsync();
                    if (orderStages != null)
                    foreach (var item in orderStages)
                    {
                        this.Data.Add(item);
                    }

                    break;
                case ViewsEnum.LeadStatus:
                    var leadStatuses = await this._leadStatusDataService.GetAllLeadStatussAsync();
                    if (leadStatuses != null)
                        foreach (var item in leadStatuses)
                        {
                            this.Data.Add(item);
                        }

                    break;
                default:
                    MessageBox.Show("No view selected");
                    break;
            }

            this.TextBoxValue = string.Empty;
        }

        public string TextBoxValue { get => textBoxValue; set => Set(ref textBoxValue, value, nameof(TextBoxValue)); }

        // This is the model used for the data entry form (Create/Update)
        private object _selectedData;
        public object SelectedData
        {
            get => _selectedData;
            set => Set(ref _selectedData, value, nameof(SelectedData));
        }

        // Data Properties
        private ObservableCollection<object> _data = new ObservableCollection<object>();
        public ObservableCollection<object> Data
        {
            get => _data;
            set => Set(ref _data, value, nameof(Data));
        }

        private async Task AddButtonCommandExecute(object obj)
        {
            var viewName = (ViewsEnum)obj;

            switch (viewName)
            {
                case ViewsEnum.DeadReason:                    

                    if (this.TextBoxValue.Contains(','))
                    {
                        var multival = this.TextBoxValue.Split(',');
                        foreach (var item in multival)
                        {
                            if (!string.IsNullOrEmpty(item) || !string.IsNullOrWhiteSpace(item))
                            {                                
                                var model = new DeadReasonModel { Name = item.Trim() };
                                await this._deadReasonDataService.CreateDeadReasonAsync(model);
                                this.Data.Add(model);
                            }
                        }
                    }
                    else
                    {
                        var model = new DeadReasonModel { Name = this.TextBoxValue.Trim() };
                        await this._deadReasonDataService.CreateDeadReasonAsync(model);
                        this.Data.Add(model);
                    }
                    break;
                case ViewsEnum.MatureStages:
                    
                    if (this.TextBoxValue.Contains(','))
                    {
                        var multival = this.TextBoxValue.Split(',');
                        foreach (var item in multival)
                        {
                            if (!string.IsNullOrEmpty(item) || !string.IsNullOrWhiteSpace(item))
                            {
                                var model = new MatureStagesModel { Name = item.Trim() };
                                await this._matureStagesDataService.CreateMatureStagesAsync(model);
                                this.Data.Add(model);
                            }
                        }
                    }
                    else
                    {
                        var model = new MatureStagesModel { Name = this.TextBoxValue.Trim() };
                        await this._matureStagesDataService.CreateMatureStagesAsync(model);
                        this.Data.Add(model);
                    }
                    break;
                case ViewsEnum.LeadSource:
                    
                    if (this.TextBoxValue.Contains(','))
                    {
                        var multival = this.TextBoxValue.Split(',');
                        foreach (var item in multival)
                        {
                            if (!string.IsNullOrEmpty(item) || !string.IsNullOrWhiteSpace(item))
                            {
                                var model = new LeadSourceModel { Name = item.Trim() };
                                await this._leadSourceDataService.CreateLeadSourceAsync(model);
                                this.Data.Add(model);
                            }
                        }
                    }
                    else
                    {
                        var model = new LeadSourceModel { Name = this.TextBoxValue.Trim() };
                        await this._leadSourceDataService.CreateLeadSourceAsync(model);
                        this.Data.Add(model);
                    }
                    break;
                case ViewsEnum.LeadLabels:
                    
                    if (this.TextBoxValue.Contains(','))
                    {
                        var multival = this.TextBoxValue.Split(',');
                        foreach (var item in multival)
                        {
                            if (!string.IsNullOrEmpty(item) || !string.IsNullOrWhiteSpace(item))
                            {
                                var model = new LeadLabelsModel { Name = item.Trim() };
                                await this._leadLabelsDataService.CreateLeadLabelsAsync(model);
                                this.Data.Add(model);
                            }
                        }
                    }
                    else
                    {
                        var model = new LeadLabelsModel { Name = this.TextBoxValue.Trim() };
                        await this._leadLabelsDataService.CreateLeadLabelsAsync(model);
                        this.Data.Add(model);
                    }
                    break;
                case ViewsEnum.LeadTags:
                    
                    if (this.TextBoxValue.Contains(','))
                    {
                        var multival = this.TextBoxValue.Split(',');
                        foreach (var item in multival)
                        {
                            if (!string.IsNullOrEmpty(item) || !string.IsNullOrWhiteSpace(item))
                            {
                                var model = new LeadTagModel { Name = item.Trim() };
                                await this._leadTagsDataService.CreateLeadTagAsync(model);
                                this.Data.Add(model);
                            }
                        }
                    }
                    else
                    {
                        var model = new LeadTagModel { Name = this.TextBoxValue.Trim() };
                        await this._leadTagsDataService.CreateLeadTagAsync(model);
                        this.Data.Add(model);
                    }
                    break;
                case ViewsEnum.FollowUpStages:
                    
                    if (this.TextBoxValue.Contains(','))
                    {
                        var multival = this.TextBoxValue.Split(',');
                        foreach (var item in multival)
                        {
                            if (!string.IsNullOrEmpty(item) || !string.IsNullOrWhiteSpace(item))
                            {
                                var model = new FollowUpStagesModel { Name = item.Trim() };
                                await this._followUpStagesDataService.CreateFollowUpStagesAsync(model);
                                this.Data.Add(model);
                            }
                        }
                    }
                    else
                    {
                        var model = new FollowUpStagesModel { Name = this.TextBoxValue.Trim() };
                        await this._followUpStagesDataService.CreateFollowUpStagesAsync(model);
                        this.Data.Add(model);
                    }
                    break;
                case ViewsEnum.Departments:
                    
                    if (this.TextBoxValue.Contains(','))
                    {
                        var multival = this.TextBoxValue.Split(',');
                        foreach (var item in multival)
                        {
                            if (!string.IsNullOrEmpty(item) || !string.IsNullOrWhiteSpace(item))
                            {
                                var model = new DepartmentsModel { Name = item.Trim() };
                                await this._departmentsDataService.CreateDepartmentsAsync(model);
                                this.Data.Add(model);
                            }
                        }
                    }
                    else
                    {
                        var model = new DepartmentsModel { Name = this.TextBoxValue.Trim() };
                        await this._departmentsDataService.CreateDepartmentsAsync(model);
                        this.Data.Add(model);
                    }
                    break;
                case ViewsEnum.OrderStages:
                    
                    if (this.TextBoxValue.Contains(','))
                    {
                        var multival = this.TextBoxValue.Split(',');
                        foreach (var item in multival)
                        {
                            if (!string.IsNullOrEmpty(item) || !string.IsNullOrWhiteSpace(item))
                            {
                                var model = new OrderStageModel { Name = item.Trim() };
                                await this._orderStagesDataService.CreateOrderStageAsync(model);
                                this.Data.Add(model);
                            }
                        }
                    }
                    else
                    {
                        var model = new OrderStageModel { Name = this.TextBoxValue.Trim() };
                        await this._orderStagesDataService.CreateOrderStageAsync(model);
                        this.Data.Add(model);
                    }
                    break;
                case ViewsEnum.LeadStatus:

                    if (this.TextBoxValue.Contains(','))
                    {
                        var multival = this.TextBoxValue.Split(',');
                        foreach (var item in multival)
                        {
                            if (!string.IsNullOrEmpty(item) || !string.IsNullOrWhiteSpace(item))
                            {
                                var model = new LeadStatusModel { Name = item.Trim() };
                                await this._leadStatusDataService.CreateLeadStatusAsync(model);
                                this.Data.Add(model);
                            }
                        }
                    }
                    else
                    {
                        var model = new LeadStatusModel { Name = this.TextBoxValue.Trim() };
                        await this._leadStatusDataService.CreateLeadStatusAsync(model);
                        this.Data.Add(model);
                    }
                    break;
                default:
                    MessageBox.Show("No view selected");
                    break;
            }

            this.TextBoxValue = string.Empty;
        }

        private bool CanSaveLead(object arg)
        {
            // Basic validation: must have First Name and Email
            return true;
        }

        private void LoadDynamicView(object obj)
        {
            var viewName = (ViewsEnum)obj;
            if (viewName != null)
            {
                var view = new SettingsCommonView();
                view.DataContext = this;
                view.DynamicView = viewName;
                view.ControlTitle = viewName.ToString();
                var window = new Window();
                window.Title = viewName.ToString();
                window.Content = view;
                window.Owner = App.Current.MainWindow;
                window.Width = 800;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                window.Show();
            }
        }
    }
}
