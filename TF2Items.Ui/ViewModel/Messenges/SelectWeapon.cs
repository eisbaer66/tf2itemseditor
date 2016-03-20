using GalaSoft.MvvmLight.Messaging;
using TF2Items.Core;

namespace TF2Items.Ui.ViewModel.Messenges
{
    public class SelectWeapon : MessageBase
    {
        public Tf2Weapon Weapon { get; set; }
    }
}