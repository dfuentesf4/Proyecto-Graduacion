using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Accounting.PettyCashSummary
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel(PettyCashSummaryApiClient pettyCashSummaryApiClient)
        {
            PettyCashSummaryApiClient = pettyCashSummaryApiClient;
            LoadPettyCashSummaries();
        }

        public PettyCashSummaryApiClient PettyCashSummaryApiClient { get; set; }
        public ICommand DeleteCommand => new Command<HFPMapp.Models.PettyCashSummary>(DeletePettyCashSummary);
        public ICommand EditCommand => new Command<HFPMapp.Models.PettyCashSummary>(EditPettyCashSummary);
        public ICommand RefreshCommand => new Command(async () => await LoadPettyCashSummaries());
        public ICommand SearchCommand => new Command(async () => await FilterPettyCashSummariesAsync());
        public ICommand CreateCommand => new Command(async () => await Shell.Current.GoToAsync("CreatePettyCashSummary"));

        private List<HFPMapp.Models.PettyCashSummary> _allPettyCashSummaries;
        public List<HFPMapp.Models.PettyCashSummary> AllPettyCashSummaries
        {
            get => _allPettyCashSummaries;
            set
            {
                _allPettyCashSummaries = value;
                OnPropertyChanged(nameof(AllPettyCashSummaries));
            }
        }

        private List<HFPMapp.Models.PettyCashSummary> _pettyCashSummaries;
        public List<HFPMapp.Models.PettyCashSummary> PettyCashSummaries
        {
            get => _pettyCashSummaries;
            set
            {
                _pettyCashSummaries = value;
                OnPropertyChanged(nameof(PettyCashSummaries));
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
                FilterPettyCashSummaries(); // Filtra automáticamente cuando se actualiza el texto de búsqueda
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
                FilterPettyCashSummaries(); // Filtra automáticamente cuando se actualiza el estado de ver inactivos
            }
        }

        public async Task LoadPettyCashSummaries()
        {
            IsRefreshing = true;
            AllPettyCashSummaries = await PettyCashSummaryApiClient.GetPettyCashSummariesAsync();
            FilterPettyCashSummaries();
            IsRefreshing = false;
        }

        public async void DeletePettyCashSummary(HFPMapp.Models.PettyCashSummary pettyCashSummary)
        {
            pettyCashSummary.IsActive = false;
            bool response = await PettyCashSummaryApiClient.UpdatePettyCashSummaryAsync(pettyCashSummary);
            if (response)
                ThrowMessage.ShowSuccessMessage("Resumen de Caja Chica eliminado correctamente");
            else
                ThrowMessage.ShowErrorMessage("Error al eliminar el Resumen de Caja Chica");

            await LoadPettyCashSummaries();
        }

        public async void EditPettyCashSummary(HFPMapp.Models.PettyCashSummary pettyCashSummary)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "PettyCashSummary", pettyCashSummary }
            };

            await Shell.Current.GoToAsync("EditPettyCashSummary", parameters);
        }

        public async Task FilterPettyCashSummariesAsync()
        {
            try
            {
                FilterPettyCashSummaries();
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to filter petty cash summaries: " + ex.Message);
            }
        }

        private void FilterPettyCashSummaries()
        {
            var filteredSummaries = AllPettyCashSummaries;

            // Filtrar resúmenes por el texto de búsqueda en Descripción, Año, o Mes
            if (!string.IsNullOrEmpty(SearchText))
            {
                filteredSummaries = filteredSummaries.Where(ps =>
                    (!string.IsNullOrEmpty(ps.Description) && ps.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (ps.Year.HasValue && ps.Year.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (ps.Month.HasValue && ps.Month.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            if (SeeInactives)
            {
                PettyCashSummaries = filteredSummaries.Where(ps => ps.IsActive == false).ToList();
            }
            else
            {
                PettyCashSummaries = filteredSummaries.Where(ps => ps.IsActive == true).ToList();
            }
        }
    }
}
