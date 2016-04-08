using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using TF2Items.Ui.ViewModel;

namespace TF2Items.Ui.Services
{
    public interface INotificationService
    {}

    public class SimpleNotificationService : INotificationService
    {
        public SimpleNotificationService(IMessenger messenger)
        {
            messenger.Register<ToastMessage>(this, Handle);
        }

        private void Handle(ToastMessage message)
        {
            MessageBox.Show(message.Text, message.Caption);
        }
    }
}