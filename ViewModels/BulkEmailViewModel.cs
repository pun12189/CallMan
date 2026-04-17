using BahiKitab.Core;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BahiKitab.ViewModels
{
    public class BulkEmailViewModel : ViewModelBase
    {
        private string subject;
        private string body;
        private string attachmentPath;

        public string Subject { get => subject; set => Set(ref subject, value, nameof(Subject)); }
        public string Body { get => body; set => Set(ref body, value, nameof(Body)); }
        public string AttachmentPath => Path.GetFileName(attachmentPath) ?? "No file attached";

        public ICommand AttachFileCommand => new RelayCommand(_ =>
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                attachmentPath = dlg.FileName;
                long fileSize = new FileInfo(attachmentPath).Length;
                if (fileSize > 25 * 1024 * 1024)
                {
                    MessageBox.Show("File is too large for SMTP sending.");
                    return;
                }

                OnPropertyChanged(nameof(AttachmentPath));
            }
        });
    }
}
