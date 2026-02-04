using BahiKitab.Models;
using BahiKitab.Services;
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
using System.Windows.Shapes;

namespace BahiKitab.Views
{
    /// <summary>
    /// Interaction logic for ImageViewer.xaml
    /// </summary>
    public partial class ImageViewer : Window
    {
        private readonly ImagesDataService imagesDataService;
        private readonly List<int> imgIds;

        public ImageViewer(List<int> ids)
        {
            InitializeComponent();
            imagesDataService = new ImagesDataService();
            this.imgIds = ids;
            this.Loaded += this.ImageViewer_Loaded;
        }

        private async void ImageViewer_Loaded(object sender, RoutedEventArgs e)
        {
            var list = new List<ImageModel>();
            if (imgIds != null && imgIds.Count > 0) 
            {
                foreach (int id in imgIds)
                {
                    list.Add(await imagesDataService.GetImage(id));
                }
            }

            this.ThumbnailList.ItemsSource = list;
        }
    }
}
