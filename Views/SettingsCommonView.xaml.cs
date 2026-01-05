using BahiKitab.Helper;
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
    /// Interaction logic for SettingsCommonView.xaml
    /// </summary>
    public partial class SettingsCommonView : UserControl
    {
        // Using a DependencyProperty as the backing store for DynamicView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DynamicViewProperty =
            DependencyProperty.Register("DynamicView", typeof(ViewsEnum), typeof(SettingsCommonView), new PropertyMetadata(ViewsEnum.DeadReason));

        // Using a DependencyProperty as the backing store for ControlTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControlTitleProperty =
            DependencyProperty.Register("ControlTitle", typeof(string), typeof(SettingsCommonView), new PropertyMetadata(string.Empty));

        public SettingsCommonView()
        {
            InitializeComponent();
        }

        public string ControlTitle
        {
            get { return (string)GetValue(ControlTitleProperty); }
            set { SetValue(ControlTitleProperty, value); }
        }

        public ViewsEnum DynamicView
        {
            get { return (ViewsEnum)GetValue(DynamicViewProperty); }
            set { SetValue(DynamicViewProperty, value); }
        }
    }
}
