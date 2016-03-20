using System.Windows;
using System.Windows.Controls;
using TF2Items.Core;
using TF2Items.Ui.ViewModel;

namespace TF2Items.Ui.Views
{
    public class WeaponDetailsAttributeTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {        
            FrameworkElement fe = container as FrameworkElement;
            IWeaponDetailsAttributeViewModel vm = item as IWeaponDetailsAttributeViewModel;

            switch (vm.Class.EditMode)
            {
                case AttributeEditing.Set:
                    return fe.FindResource("SetAttributeTemplate") as DataTemplate;
                case AttributeEditing.Numerical:
                    return fe.FindResource("NumericalAttributeTemplate") as DataTemplate;
                default:
                    return fe.FindResource("NumericalAttributeTemplate") as DataTemplate;

            }
        }
    }
}