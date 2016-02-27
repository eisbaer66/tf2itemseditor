using Ninject.Extensions.Conventions;
using Ninject.Modules;
using TF2Items.Core;
using TF2Items.Parsers;
using TF2Items.Ui.Services;
using TF2Items.Ui.ViewModel;
using TF2Items.ValvePak;

namespace TF2Items.Ui.DesignTime
{
    public class DesignTimeModule : RunTimeModule
    {
        public override void Load()
        {
            base.Load();

            Rebind<ISteamConfig>().To<SteamConfig>().InSingletonScope();
            Rebind<IWeaponIconConfig>().To<WeaponIconConfig>().InSingletonScope();
            Rebind<IConfig>().To<Config>().InSingletonScope();
            Rebind<ITf2WeaponService>().To<Tf2WeaponService>().InSingletonScope();
            Bind<ITf2WeaponService>().To<Services.Tf2WeaponService>().WhenInjectedInto<Tf2WeaponService>();
        }
    }
}