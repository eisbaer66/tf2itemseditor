using TF2Items.Core;

namespace TF2Items.Ui.ViewModel
{
    public interface IWeaponDetailsAttributeViewModel
    {
        AttributeClass Class { get; set; }
        bool Editing { get; set; }
        ConfigWeaponAttribute WeaponAttribute { get; set; }
        Tf2Attribute Attribute { get; set; }
    }
}