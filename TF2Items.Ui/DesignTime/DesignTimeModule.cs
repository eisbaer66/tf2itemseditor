using Ninject.Modules;
using TF2Items.Core;
using TF2Items.Parsers;
using TF2Items.Ui.Services;
using TF2Items.ValvePak;

namespace TF2Items.Ui.DesignTime
{
    public class DesignTimeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISteamConfig>().To<SteamConfig>();
            Bind<IWeaponIconConfig>().To<WeaponIconConfig>();
            Bind<IWeaponIconService>().To<WeaponIconService>();
            Bind<IConfig>().To<Config>();
            Bind<IValvePakService>().To<ValvePakService>();
            Bind<IValveTextureFormatService>().To<ValveTextureFormatService>();
            Bind<ISettingsService>().To<SettingsService>();
            Bind<ITf2WeaponService>().To<Tf2WeaponService>();
            Bind<ITf2WeaponService>().To<Services.Tf2WeaponService>().WhenInjectedInto<Tf2WeaponService>();
            Bind<IStatsParser>().To<StatsParser>();
            Bind<IItemsGameWeaponsParser>().To<Parsers.ItemsGameWeaponsParser>();
            Bind<IItemsGamePrefabsParser>().To<ItemsGamePrefabsParser>();
        }
    }
}