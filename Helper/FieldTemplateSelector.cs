using BahiKitab.Models;
using System.Windows;
using System.Windows.Controls;

namespace BahiKitab.Helper
{
    public class FieldTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextTemplate { get; set; }
        public DataTemplate DropdownTemplate { get; set; }
        public DataTemplate CheckTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is DynamicField field)
            {
                return field.ControlType switch
                {
                    "Text" => TextTemplate,
                    "Dropdown" => DropdownTemplate,
                    "Check" => CheckTemplate,
                    _ => base.SelectTemplate(item, container)
                };
            }

            return base.SelectTemplate(item, container);
        }                                                                                                              
    }
}
