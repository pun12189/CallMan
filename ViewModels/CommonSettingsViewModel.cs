using BahiKitab.Core;
using BahiKitab.Helper;
using BahiKitab.Services;
using BahiKitab.Views;
using System;
using System.Collections.Generic;
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

        // Dynamic Views Command call from Admin View Model

        public ICommand DynamicViewCommand { get; set; }

        public ICommand DynamicAddViewCommand { get; set; }

        // Constructor

        public CommonSettingsViewModel()
        {
            this.DynamicViewCommand = new RelayCommand(LoadDynamicView);
            this.DynamicAddViewCommand = new RelayCommand(AddButtonCommandExecute);
        }

        private void AddButtonCommandExecute(object obj)
        {
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
