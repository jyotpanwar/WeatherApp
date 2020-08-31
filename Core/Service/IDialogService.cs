using System;
using Xamarin.Forms;

namespace Core.Service
{
    public interface IDialogService
    {
        void ShowMessage(string title, string message, string dismissButtonTitle, Action dismissed);

        void Confirm(string title, string message, string okButtonTitle, string dismissButtonTitle, Action confirmed, Action dismissed);
    }

    public class DialogpService : IDialogService
    {
        public async void Confirm(string title, string message, string okButtonTitle, string dismissButtonTitle, Action confirmed, Action dismissed)
        {
            var response = await Application.Current.MainPage.DisplayAlert(title, message, okButtonTitle, dismissButtonTitle);
            if (response == true)
            {
                confirmed?.Invoke();
            }
            else
            {
                dismissed?.Invoke();
            }
        }

        public async void ShowMessage(string title, string message, string dismissButtonTitle, Action dismissed)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, dismissButtonTitle);
            dismissed?.Invoke();
        }
    }
}
