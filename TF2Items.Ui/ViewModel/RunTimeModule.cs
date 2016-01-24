using Ninject.Modules;
using TF2Items.Core;
using TF2Items.Parsers;
using TF2Items.Ui.Services;

namespace TF2Items.Ui.ViewModel
{
    public class RunTimeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISteamConfig>().To<SteamConfig>();
            Bind<ISettingsService>().To<SettingsService>();
            Bind<ITf2WeaponService>().To<Tf2WeaponService>();
            Bind<IItemsGameWeaponsParser>().To<ItemsGameWeaponsParser>();
        }
    }
}