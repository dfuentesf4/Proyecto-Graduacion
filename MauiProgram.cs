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
            services.AddSingleton<DonorApiClient>();
            services.AddSingleton<ProjectApiClient>();
            services.AddSingleton<BeneficiaryApiClient>();
            services.AddSingleton<VolunteerApiClient>();
            services.AddSingleton<ReportApiClient>();
            services.AddSingleton<ActivityApiClient>();

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

            //Services required for Donors
            services.AddSingleton<HFPMapp.ViewModels.Donors.CreateViewModel>();
            services.AddSingleton<HFPMapp.Views.Donors.CreateView>();
            services.AddSingleton<HFPMapp.ViewModels.Donors.ListViewModel>();
            services.AddSingleton<HFPMapp.Views.Donors.ListView>();
            services.AddSingleton<HFPMapp.ViewModels.Donors.EditViewModel>();
            services.AddSingleton<HFPMapp.Views.Donors.EditView>();

            //Services required for Projects
            services.AddSingleton<HFPMapp.ViewModels.Projects.MenuViewModel>();
            services.AddSingleton<HFPMapp.Views.Projects.MenuView>();
            services.AddSingleton<HFPMapp.ViewModels.Projects.ProjectsCreateViewModel>();
            services.AddSingleton<HFPMapp.Views.Projects.ProyectsCreateView>();
            services.AddSingleton<HFPMapp.ViewModels.Projects.ProjectsListViewModel>();
            services.AddSingleton<HFPMapp.Views.Projects.ProjectsListView>();
            services.AddSingleton<HFPMapp.ViewModels.Projects.ProjectEditViewModel>();
            services.AddSingleton<HFPMapp.Views.Projects.ProjectEditView>();

            //Services required for beneficiaries
            services.AddSingleton<HFPMapp.ViewModels.Projects.Beneficiaries.CreateViewModel>();
            services.AddSingleton<HFPMapp.Views.Projects.Beneficiaries.CreateView>();
            services.AddSingleton<HFPMapp.ViewModels.Projects.Beneficiaries.ListViewModel>();
            services.AddSingleton<HFPMapp.Views.Projects.Beneficiaries.ListView>();
            services.AddSingleton<HFPMapp.ViewModels.Projects.Beneficiaries.EditViewModel>();
            services.AddSingleton<HFPMapp.Views.Projects.Beneficiaries.EditView>();

            //Services required for Volunteers
            services.AddSingleton<HFPMapp.ViewModels.Projects.Volunteers.CreateViewModel>();
            services.AddSingleton<HFPMapp.Views.Projects.Volunteers.CreateView>();
            services.AddSingleton<HFPMapp.ViewModels.Projects.Volunteers.ListViewModel>();
            services.AddSingleton<HFPMapp.Views.Projects.Volunteers.ListView>();
            services.AddSingleton<HFPMapp.ViewModels.Projects.Volunteers.EditViewModel>();
            services.AddSingleton<HFPMapp.Views.Projects.Volunteers.EditView>();

            //Services required for Reports
            services.AddSingleton<HFPMapp.ViewModels.Projects.Reports.CreateViewModel>();
            services.AddSingleton<HFPMapp.Views.Projects.Reports.CreateView>();
            services.AddSingleton<HFPMapp.ViewModels.Projects.Reports.ListViewModel>();
            services.AddSingleton<HFPMapp.Views.Projects.Reports.ListView>();
            services.AddSingleton<HFPMapp.ViewModels.Projects.Reports.EditViewModel>();
            services.AddSingleton<HFPMapp.Views.Projects.Reports.EditView>();

            //Services required for Activities
            services.AddSingleton<HFPMapp.ViewModels.Projects.Activities.CreateViewModel>();
            services.AddSingleton<HFPMapp.Views.Projects.Activities.CreateView>();
            services.AddSingleton<HFPMapp.ViewModels.Projects.Activities.ListViewModel>();
            services.AddSingleton<HFPMapp.Views.Projects.Activities.ListView>();
            services.AddSingleton<HFPMapp.ViewModels.Projects.Activities.EditViewModel>();
            services.AddSingleton<HFPMapp.Views.Projects.Activities.EditView>();


            return services;
        }
    }
}
