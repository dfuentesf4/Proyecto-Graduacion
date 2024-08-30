using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.ViewModels.Settings
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            string? theme = SecureStorage.GetAsync("APP_THEME").GetAwaiter().GetResult();
            if (theme is null) { }
            else if (theme.Equals("System")) IsSystemThemeCheked = true;
            else if (theme.Equals("Dark")) IsDarkThemeCheked = true;
            else if (theme.Equals("Ligth")) IsLigthThemeCheked = true;
        }

        private bool _IsSystemThemeCheked;
        public bool IsSystemThemeCheked
        {
            get
            {
                return _IsSystemThemeCheked;
            }
            set
            {
                _IsSystemThemeCheked = value;
                if (value == true)
                {
                    SecureStorage.SetAsync("APP_THEME", "System");
                    Application.Current.UserAppTheme = AppInfo.RequestedTheme;
                }
                OnPropertyChanged(nameof(IsSystemThemeCheked));
            }
        }

        private bool _IsDarkThemeCheked;
        public bool IsDarkThemeCheked
        {
            get
            {
                return _IsDarkThemeCheked;
            }
            set
            {
                _IsDarkThemeCheked = value;
                if (value == true)
                {
                    SecureStorage.SetAsync("APP_THEME", "Dark");
                    Application.Current.UserAppTheme = AppTheme.Dark;
                }
                OnPropertyChanged(nameof(IsDarkThemeCheked));
            }
        }

        private bool _IsLigthThemeCheked;
        public bool IsLigthThemeCheked
        {
            get
            {
                return _IsLigthThemeCheked;
            }
            set
            {
                _IsLigthThemeCheked = value;
                if (value == true)
                {
                    SecureStorage.SetAsync("APP_THEME", "Ligth");
                    Application.Current.UserAppTheme = AppTheme.Light;
                }
                OnPropertyChanged(nameof(IsLigthThemeCheked));
            }
        }
    }
}
