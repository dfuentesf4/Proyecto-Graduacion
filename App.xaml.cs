namespace HFPMapp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            await Shell.Current.GoToAsync("//Accounting");
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
            base.OnStart();

            try
            {
                string? theme = SecureStorage.GetAsync("APP_THEME").GetAwaiter().GetResult();

                if (theme is null) Application.Current.UserAppTheme = AppInfo.RequestedTheme;
                else if (theme.Equals("System")) Application.Current.UserAppTheme = AppInfo.RequestedTheme;
                else if (theme.Equals("Dark")) Application.Current.UserAppTheme = AppTheme.Dark;
                else if (theme.Equals("Ligth")) Application.Current.UserAppTheme = AppTheme.Light;
                else Application.Current.UserAppTheme = AppInfo.RequestedTheme;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
