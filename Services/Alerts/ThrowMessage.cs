using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Services.Alerts
{
    public static class ThrowMessage
    {
        public static void ShowErrorMessage(string message)
        {
            App.Current.MainPage.DisplayAlert("Error", message, "Ok");
        }

        public static void ShowSuccessMessage(string message)
        {
            App.Current.MainPage.DisplayAlert("Exitoso", message, "Ok");
        }
    }
}
