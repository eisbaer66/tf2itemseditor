using Ninject.Modules;
using TF2Items.Core;
using TF2Items.Parsers;
using TF2Items.Ui.Services;
using TF2Items.ValvePak;

namespace TF2Items.Ui.ViewModel
{
    public class RunTimeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISteamConfig>().To<SteamConfig>();
            Bind<IWeaponIconConfig>().To<WeaponIconConfig>();
            Bind<IWeaponIconService>().To<WeaponIconService>();
            Bind<TF2Items.ValvePak.IConfig>().To<TF2Items.ValvePak.Config>();
            Bind<IValvePakService>().To<ValvePakService>();
            Bind<IValveTextureFormatService>().To<ValveTextureFormatService>();
            Bind<ISettingsService>().To<SettingsService>();
            Bind<ITf2WeaponService>().To<Tf2WeaponService>();
            Bind<IItemsGameWeaponsParser>().To<ItemsGameWeaponsParser>();
        }
    }
}