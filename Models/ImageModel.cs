using BahiKitab.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BahiKitab.Models
{
    public class ImageModel : ObservableObject
    {
        private int id;
        private string fileName;
        private BitmapSource imageSource;

        public int Id { get => id; set => Set(ref id, value, nameof(Id)); }
        public string FileName { get => fileName; set => Set(ref fileName, value, nameof(FileName)); }
        public BitmapSource ImageSource { get => imageSource; set => Set(ref imageSource, value, nameof(ImageSource)); }
    }
}
