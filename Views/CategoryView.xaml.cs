using BahiKitab.Models;
using BahiKitab.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BahiKitab.Views
{
    /// <summary>
    /// Interaction logic for CategoryView.xaml
    /// </summary>
    public partial class CategoryView : UserControl
    {
        private CategoryViewModel _viewModel;

        public CategoryView()
        {
            InitializeComponent();
            this.Loaded += CategoryView_Loaded;            
        }

        private void CategoryView_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as CategoryViewModel;
            if (vm != null) { 
                _viewModel = vm;
            }
        }

        private void AddSub_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                if (sender is Button btn && btn.DataContext is CategoryModel selected)
                {
                    // For simplicity, using a quick input prompt logic
                    string newName = Microsoft.VisualBasic.Interaction.InputBox(
                        "Enter Sub Category Name:",
                        "Sub Category",
                        selected.Name);

                    if (!string.IsNullOrWhiteSpace(newName) && newName != selected.Name)
                    {
                        var arr = new object[] { selected, newName };

                        _viewModel.AddSubCategoryCommand.Execute(arr); // Push to MySQL
                    }
                }
            }                
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                if (CategoryTree.SelectedItem is CategoryModel selected)
                {
                    _viewModel.DeleteCategoryCommand.Execute(selected);
                }
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // Get the category from the button's DataContext
            if (sender is Button btn && btn.DataContext is CategoryModel selected)
            {
                // For simplicity, using a quick input prompt logic
                string newName = Microsoft.VisualBasic.Interaction.InputBox(
                    "Enter New Category Name:",
                    "Edit Category",
                    selected.Name);

                if (!string.IsNullOrWhiteSpace(newName) && newName != selected.Name)
                {
                    selected.Name = newName; // UI updates automatically via PropertyChanged
                    _viewModel.UpdateCategoryCommand.Execute(selected); // Push to MySQL
                }
            }
        }

        private void AddRoot_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                    // For simplicity, using a quick input prompt logic
                    string newName = Microsoft.VisualBasic.Interaction.InputBox(
                        "Enter Main Category Name:",
                        "Main Category");

                    if (!string.IsNullOrWhiteSpace(newName))
                    { 
                        _viewModel.AddCategoryCommand.Execute(newName); // Push to MySQL
                    }
            }
        }
    }
}
