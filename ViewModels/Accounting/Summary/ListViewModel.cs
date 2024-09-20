using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Accounting.Summary
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel(SummaryApiClient summaryApiClient)
        {
            SummaryApiClient = summaryApiClient;
            LoadSummaries();
        }

        public SummaryApiClient SummaryApiClient { get; set; }
        public ICommand DeleteCommand => new Command<HFPMapp.Models.Summary>(DeleteSummary);
        public ICommand EditCommand => new Command<HFPMapp.Models.Summary>(EditSummary);
        public ICommand RefreshCommand => new Command(async () => await LoadSummaries());
        public ICommand SearchCommand => new Command(async () => await FilterSummaryAsync());
        public ICommand CreateCommand => new Command(async () => await Shell.Current.GoToAsync("CreateSummary"));

        private List<HFPMapp.Models.Summary> _allSummaries;
        public List<HFPMapp.Models.Summary> AllSummaries
        {
            get => _allSummaries;
            set
            {
                _allSummaries = value;
                OnPropertyChanged(nameof(AllSummaries));
            }
        }

        private List<HFPMapp.Models.Summary> _summaries;
        public List<HFPMapp.Models.Summary> Summaries
        {
            get => _summaries;
            set
            {
                _summaries = value;
                OnPropertyChanged(nameof(Summaries));
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

        public async Task LoadSummaries()
        {
            IsRefreshing = true;
            AllSummaries = await SummaryApiClient.GetSummariesAsync();
            FilterSummaries();
            IsRefreshing = false;
        }

        public async void DeleteSummary(HFPMapp.Models.Summary summary)
        {
            summary.IsActive = false;
            bool response = await SummaryApiClient.UpdateSummaryAsync(summary);
            if (response) ThrowMessage.ShowSuccessMessage("Resumen eliminado correctamente");
            else ThrowMessage.ShowErrorMessage("Error al eliminar el resumen");
            await LoadSummaries();
        }

        public async void EditSummary(HFPMapp.Models.Summary summary)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "Summary" , summary }
            };

            await Shell.Current.GoToAsync("EditSummary", parameters);
        }

        public async Task FilterSummaryAsync()
        {
            try
            {
                FilterSummaries();
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to filter summaries: " + ex.Message);
            }
        }

        private void FilterSummaries()
        {
            var filteredSummaries = AllSummaries;

            // Filtrar resúmenes por el texto de búsqueda en Año, Mes, o Flujos de Salida
            if (!string.IsNullOrEmpty(SearchText))
            {
                filteredSummaries = filteredSummaries.Where(s =>
                    (s.Year.HasValue && s.Year.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (s.Month.HasValue && s.Month.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (s.Outflows.HasValue && s.Outflows.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            if (SeeInactives)
            {
                Summaries = filteredSummaries.Where(s => s.IsActive == false).ToList();
            }
            else
            {
                Summaries = filteredSummaries.Where(s => s.IsActive == true).ToList();
            }
        }
    }
}
