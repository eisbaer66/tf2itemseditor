/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:TF2Items.Ui"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator.NinjectAdapter.Unofficial;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using Ninject.Extensions.Factory;
using TF2Items.Ui.Services;

namespace TF2Items.Ui.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            NinjectSettings ninjectSettings = new NinjectSettings { LoadExtensions = false };
            //StandardKernel kernel = new StandardKernel(ninjectSettings, new DesignTime.DesignTimeModule(), new FuncModule());
            StandardKernel kernel = ViewModelBase.IsInDesignModeStatic
                ? new StandardKernel(ninjectSettings, new DesignTime.DesignTimeModule(), new FuncModule())
                : new StandardKernel(ninjectSettings, new RunTimeModule(), new FuncModule());
            
            NinjectServiceLocator ninjectServiceLocator = new NinjectServiceLocator(kernel);
            ServiceLocator.SetLocatorProvider(() => ninjectServiceLocator);

            kernel.Get<INotificationService>();
            kernel.Get<IAutomationService>().Startup();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}