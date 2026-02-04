using BahiKitab.Core;
using BahiKitab.Models;
using BahiKitab.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BahiKitab.ViewModels
{
    public class CategoryViewModel : ViewModelBase
    {
        private readonly CategoryDataService categoryDataService;

        private ObservableCollection<CategoryModel> rootCategories = new();

        public ObservableCollection<CategoryModel> RootCategories { get => rootCategories; set => Set(ref rootCategories, value, nameof(RootCategories)); }

        public RelayCommand AddCategoryCommand { get; private set; }

        public RelayCommand AddSubCategoryCommand { get; private set; }

        public RelayCommand DeleteCategoryCommand { get; private set; }

        public RelayCommand UpdateCategoryCommand { get; private set; }

        public RelayCommand LoadCategoryCommand { get; private set; }

        public CategoryViewModel()
        {
            this.AddCategoryCommand = new RelayCommand(AddCategoryCommandExecute);
            this.DeleteCategoryCommand = new RelayCommand(DeleteCategoryCommandExecute);
            this.UpdateCategoryCommand = new RelayCommand(UpdateCategoryCommandExecute);
            this.AddSubCategoryCommand = new RelayCommand(AddSubCategoryCommandExecute);
            this.LoadCategoryCommand = new RelayCommand(LoadCategoryCommandExecute);
            this.categoryDataService = new CategoryDataService();
            this.LoadCategoryCommand.Execute(this);
        }

        private async void LoadCategoryCommandExecute(object obj)
        {
            this.RootCategories = await categoryDataService.GetAllCategoriesAsync();
        }

        private async void UpdateCategoryCommandExecute(object obj)
        {
            var selected = obj as CategoryModel;
            if (selected != null)
            {
                await categoryDataService.UpdateCategoryAsync(selected);
            }
        }

        private async void DeleteCategoryCommandExecute(object obj)
        {
            var selected = obj as CategoryModel;
            if (selected != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete '{selected.Name}' and all its sub-categories?",
                                             "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // 2. Delete from MySQL
                    await categoryDataService.DeleteCategoryAsync(selected);

                    // 3. Remove from UI
                    if (selected.ParentId == null)
                    {
                        // It's a root node
                        this.RootCategories.Remove(selected);
                    }
                    else
                    {
                        // It's a sub-category. We need to find its parent in the tree.
                        // We can do this by searching our dictionary or traversing.
                        RemoveFromParent(this.RootCategories, selected);
                    }
                }
            }
        }

        // Helper method to find and remove the item from the nested UI collections
        private bool RemoveFromParent(ObservableCollection<CategoryModel> categories, CategoryModel target)
        {
            if (categories.Contains(target))
            {
                categories.Remove(target);
                return true;
            }

            foreach (var cat in categories)
            {
                if (RemoveFromParent(cat.SubCategories, target)) return true;
            }
            return false;
        }

        private async void AddCategoryCommandExecute(object obj)
        {
            var name = obj as string;
            if (name != null)
            {
                var newCat = new CategoryModel { Name = name };                
                newCat = await categoryDataService.CreateCategoryAsync(newCat);
                RootCategories.Add(newCat);
            }
        }

        private async void AddSubCategoryCommandExecute(object obj)
        {
            var items = obj as object[];
            if (items != null) 
            {
                var parent = items[0] as CategoryModel;
                var name = items[1] as string;
                if (parent != null && name != null)
                {
                    var newCat = new CategoryModel { Name = name, ParentId = parent.Id  };                    
                    newCat = await categoryDataService.CreateCategoryAsync(newCat);
                    parent.SubCategories.Add(newCat);
                }
            }                
        }
    }
}
