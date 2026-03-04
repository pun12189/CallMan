using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Core
{
    public class BindingProxy : System.Windows.Freezable
    {
        protected override System.Windows.Freezable CreateInstanceCore() => new BindingProxy();
        public object Data { get => GetValue(DataProperty); set => SetValue(DataProperty, value); }
        public static readonly System.Windows.DependencyProperty DataProperty =
            System.Windows.DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new System.Windows.PropertyMetadata(null));
    }
}
