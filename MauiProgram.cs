using CommunityToolkit.Maui;
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
using Serilog;
using Syncfusion.Licensing;
using System.Globalization;
using System.Reflection;
using Syncfusion.Maui.Core.Hosting;


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
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            IServiceCollection services = builder.Services;


            //LICENCIA DE SYNCFUSION
            SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NDaF5cWWtCf1NpR2JGfV5ycEVHal5UTnVXUiweQnxTdEFjUH1fcHVXQ2RcUUJ2Ww==");

            AppSettings appSettings = new AppSettings(config.GetSection("AppSettings"));
            builder.Services.AddSingleton(appSettings);


            services.AddHFPMServices(
                new LoggerConfiguration()
                    .WriteTo.Debug()
                    .WriteTo.File(Path.Combine(FileSystem.Current.AppDataDirectory, "logs2", "log.txt"), rollingInterval: RollingInterval.Day)
                    .CreateLogger());

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("es-GT");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("es-GT");


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
            services.AddSingleton<ExpensesDetailApiClient>();
            services.AddSingleton<RevenuesDetailApiClient>();
            services.AddSingleton<SummaryApiClient>();
            services.AddSingleton<PettyCashSummaryApiClient>();
            services.AddSingleton<TransfersFromUApiClient>();
            services.AddSingleton<TransfersSummaryApiClient>();
            services.AddSingleton<BankApiClient>();
            services.AddSingleton<FolderBankApiClient>();
            services.AddSingleton<BankSummaryApiClient>();
            services.AddSingleton<BankBookApiClient>();

            //Services required for Login
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<LoginView>();
            services.AddSingleton<ForgotPasswordViewModel>();
            services.AddSingleton<ForgotPasswordView>();


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

            //Services required for ExpensesDetail
            services.AddSingleton<HFPMapp.ViewModels.Accounting.MenuViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.MenuView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.ExpensesDetails.CreateViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.ExpensesDetails.CreateView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.ExpensesDetails.ListViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.ExpensesDetails.ListView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.ExpensesDetails.EditViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.ExpensesDetails.EditView>();

            //Services required for RevenuesDetail
            services.AddSingleton<HFPMapp.ViewModels.Accounting.RevenuesDetails.CreateViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.RevenuesDetails.CreateView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.RevenuesDetails.ListViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.RevenuesDetails.ListView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.RevenuesDetails.EditViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.RevenuesDetails.EditView>();

            //Services required for Summary
            services.AddSingleton<HFPMapp.ViewModels.Accounting.Summary.CreateViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.Summary.CreateView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.Summary.ListViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.Summary.ListView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.Summary.EditViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.Summary.EditView>();

            //Services required for PettyCashSummary
            services.AddSingleton<HFPMapp.ViewModels.Accounting.PettyCashSummary.CreateViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.PettyCashSummary.CreateView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.PettyCashSummary.ListViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.PettyCashSummary.ListView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.PettyCashSummary.EditViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.PettyCashSummary.EditView>();

            //Services required for TransfersFromU
            services.AddSingleton<HFPMapp.ViewModels.Accounting.TransfersFromUS.CreateViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.TransfersFromUS.CreateView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.TransfersFromUS.ListViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.TransfersFromUS.ListView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.TransfersFromUS.EditViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.TransfersFromUS.EditView>();

            //Services required for TransfersSummary
            services.AddSingleton<HFPMapp.ViewModels.Accounting.TransfersSummary.CreateViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.TransfersSummary.CreateView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.TransfersSummary.ListViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.TransfersSummary.ListView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.TransfersSummary.EditViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.TransfersSummary.EditView>();

            //Services required for Bank
            services.AddSingleton<HFPMapp.ViewModels.Accounting.Bank.CreateViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.Bank.CreateView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.Bank.ListViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.Bank.ListView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.Bank.EditViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.Bank.EditView>();

            //Services required for FolderBank
            services.AddSingleton<HFPMapp.ViewModels.Accounting.FolderBank.CreateViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.FolderBank.CreateView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.FolderBank.ListViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.FolderBank.ListView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.FolderBank.EditViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.FolderBank.EditView>();

            //Services required for BankSummary
            services.AddSingleton<HFPMapp.ViewModels.Accounting.BankSummary.CreateViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.BankSummary.CreateView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.BankSummary.ListViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.BankSummary.ListView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.BankSummary.EditViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.BankSummary.EditView>();

            //Services required for BankBook
            services.AddSingleton<HFPMapp.ViewModels.Accounting.BankBook.CreateViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.BankBook.CreateView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.BankBook.ListViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.BankBook.ListView>();
            services.AddSingleton<HFPMapp.ViewModels.Accounting.BankBook.EditViewModel>();
            services.AddSingleton<HFPMapp.Views.Accounting.BankBook.EditView>();

            //Services required for Reports
            services.AddSingleton<HFPMapp.ViewModels.Reports.MenuViewModel>();
            services.AddSingleton<HFPMapp.Views.Reports.MenuView>();
            services.AddSingleton<HFPMapp.ViewModels.Reports.Accounting.AccountingReportsViewModel>();
            services.AddSingleton<HFPMapp.Views.Reports.Accounting.AccountingReports>();
            services.AddSingleton<HFPMapp.ViewModels.Reports.Projects.ProjectsViewModel>();
            services.AddSingleton<HFPMapp.Views.Reports.Projects.ProjectsView>();


            return services;
        }
    }
}
