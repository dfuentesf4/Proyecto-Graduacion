using HFPMapp.Models;
using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Projects.Reports
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel(ReportApiClient reportApiClient)
        {
            ReportApiClient = reportApiClient;
            LoadReports();
        }

        public ReportApiClient ReportApiClient { get; set; }
        public ICommand DeleteCommand => new Command<Report>(DeleteReport);
        public ICommand EditCommand => new Command<Report>(EditReport);
        public ICommand RefreshCommand => new Command(async () => await LoadReports());
        public ICommand SearchCommand => new Command(async () => await FilterReportsAsync());
        public ICommand CreateCommand => new Command(async () => await Shell.Current.GoToAsync("CreateReport"));

        private List<Report> _allReports;
        public List<Report> AllReports
        {
            get => _allReports;
            set
            {
                _allReports = value;
                OnPropertyChanged(nameof(AllReports));
            }
        }

        private List<Report> _reports;
        public List<Report> Reports
        {
            get => _reports;
            set
            {
                _reports = value;
                OnPropertyChanged(nameof(Reports));
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private bool _seeInactives;
        public bool SeeInactives
        {
            get => _seeInactives;
            set
            {
                _seeInactives = value;
                OnPropertyChanged(nameof(SeeInactives));
            }
        }

        public async Task LoadReports()
        {
            IsRefreshing = true;
            AllReports = await ReportApiClient.GetReportsAsync();
            ApplyFilters();
            IsRefreshing = false;
        }

        public async void DeleteReport(Report report)
        {
            report.IsActive = false;
            bool response = await ReportApiClient.UpdateReportAsync(report);
            if (response)
            {
                ThrowMessage.ShowSuccessMessage("Reporte eliminado correctamente");
                await LoadReports();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Error al eliminar el Reporte");
            }
        }

        public async void EditReport(Report report)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "Report", report }
            };

            await Shell.Current.GoToAsync("EditReport", parameters);
        }

        public async Task FilterReportsAsync()
        {
            try
            {
                ApplyFilters();
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to filter reports: " + ex.Message);
            }
        }

        private void ApplyFilters()
        {
            // Filtrar los reportes según el texto de búsqueda y el estado activo/inactivo
            var filteredReports = AllReports
                .Where(r =>
                    (string.IsNullOrEmpty(SearchText) || (!string.IsNullOrEmpty(r.Description) && r.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(r.Results) && r.Results.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(r.Recommendations) && r.Recommendations.Contains(SearchText, StringComparison.OrdinalIgnoreCase)))
                )
                .ToList();

            Reports = SeeInactives ? filteredReports.Where(r => !r.IsActive ?? false).ToList() : filteredReports.Where(r => r.IsActive ?? false).ToList();
        }
    }
}
