using BahiKitab.Core;
using BahiKitab.Models;
using BahiKitab.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BahiKitab.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private CompanyProfile companyProfile = new CompanyProfile();

        private readonly CompanyProfileDataService companyProfileDataService;

        public ICommand UploadLogoCommand { get; }
        public ICommand SaveCommand { get; }

        public CompanyProfile CompanyProfile { get => companyProfile; set => Set(ref companyProfile, value, nameof(CompanyProfile)); }

        public ProfileViewModel()
        {
            companyProfileDataService = new CompanyProfileDataService();
            UploadLogoCommand = new RelayCommand(_ => {
                OpenFileDialog dlg = new OpenFileDialog { Filter = "Image files (*.png;*.jpg)|*.png;*.jpg" };
                if (dlg.ShowDialog() == true)
                {
                    var img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri(dlg.FileName);
                    img.EndInit();
                    CompanyProfile.LogoPath = img;
                }
            });

            LoadProfile();
            SaveCommand = new RelayCommand(_ => SaveCommandExecute());
        }

        private async void LoadProfile()
        {
            CompanyProfile = await companyProfileDataService.GetProfileAsync(1);
        }

        private async void SaveCommandExecute()
        {
            if (companyProfile.Id == 0)
            {
                await companyProfileDataService.CreateCompanyProfileAsync(companyProfile);
                MessageBox.Show("Company Profile Saved Successfully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                await companyProfileDataService.UpdateCompanyProfileAsync(companyProfile);
                MessageBox.Show("Company Profile Updated Successfully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}

