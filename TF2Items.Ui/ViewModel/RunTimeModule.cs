using GalaSoft.MvvmLight.Messaging;
using Ninject.Modules;
using TF2Items.Core;
using TF2Items.Parsers;
using TF2Items.ValvePak;
using Ninject.Extensions.Conventions;
using TF2Items.Ui.Services;

namespace TF2Items.Ui.ViewModel
{
    public class RunTimeModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x => x
                .FromThisAssembly()
                .IncludingNonePublicTypes()
                .SelectAllClasses()
                .NotInNamespaceOf<DesignTime.Tf2WeaponService>()
                .Join.FromAssemblyContaining<SteamConfig>()
                .SelectAllClasses()
                .Join.FromAssemblyContaining<IValvePakService>()
                .SelectAllClasses()
                .Join.FromAssemblyContaining<ItemsGameWeaponsParser>()
                .SelectAllClasses()
                .Excluding<Tf2AttributesParserCache>()
                .BindAllInterfaces()
                .Configure(c => c.InSingletonScope()));

            Bind<ITf2AttributesParser>().To<Tf2AttributesParser>().WhenInjectedExactlyInto<Tf2AttributesParserCache>();
            Bind<MainViewModel>().To<MainViewModel>().InSingletonScope();
            Bind<IMessenger>().ToConstant(Messenger.Default);
        }
    }
}