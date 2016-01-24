using Ninject.Modules;
using TF2Items.Core;
using TF2Items.Parsers;
using TF2Items.Ui.Services;

namespace TF2Items.Ui.DesignTime
{
    public class DesignTimeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISteamConfig>().To<SteamConfig>();
            Bind<ISettingsService>().To<SettingsService>();
            Bind<ITf2WeaponService>().To<Tf2WeaponService>();
            Bind<ITf2WeaponService>().To<Services.Tf2WeaponService>().WhenInjectedInto<Tf2WeaponService>();
            Bind<IItemsGameWeaponsParser>().To<Parsers.ItemsGameWeaponsParser>();
        }
    }
}