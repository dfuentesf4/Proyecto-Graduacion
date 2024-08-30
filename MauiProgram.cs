using HFPMapp.Services;
using HFPMapp.Services.AppSettings;
using HFPMapp.Services.HTTP;
using HFPMapp.ViewModels.Login;
using HFPMapp.ViewModels.Settings;
using HFPMapp.ViewModels.Users;
using HFPMapp.Views.Login;
using HFPMapp.Views.Settings;
using HFPMapp.Views.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System.Reflection;


namespace HFPMapp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("HFPMapp.appsettings.json");

            var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

            builder.Configuration.AddConfiguration(config);

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            IServiceCollection services = builder.Services;

            
            AppSettings appSettings = new AppSettings(config.GetSection("AppSettings"));
            builder.Services.AddSingleton(appSettings);

            services.AddHFPMServices(
                new LoggerConfiguration()
                    .WriteTo.Debug()
                    .WriteTo.File(Path.Combine(FileSystem.Current.AppDataDirectory, "logs2", "log.txt"), rollingInterval: RollingInterval.Day)
                    .CreateLogger());


            return builder.Build();
        }

        public static IServiceCollection AddHFPMServices(this IServiceCollection services, Serilog.ILogger logger)
        {
            services.AddSerilog(logger);

            //Service to Session
            services.AddSingleton<UserSessionService>();

            //Services http client
            services.AddSingleton<UserApiClient>();

            //Services required for Login
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<LoginView>();

            //Services required for Settings
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<SettingsView>();

            //Services required for Users
            services.AddSingleton<CreateViewModel>();
            services.AddSingleton<CreateView>();
            services.AddSingleton<ListViewModel>();
            services.AddSingleton<Views.Users.ListView>();
            services.AddSingleton<EditViewModel>();
            services.AddSingleton<EditView>();

            return services;
        }
    }
}
