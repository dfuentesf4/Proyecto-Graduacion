using HFPMapp.Models;
using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Accounting.TransfersSummary
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel(TransfersSummaryApiClient transferSummaryApiClient)
        {
            TransferSummaryApiClient = transferSummaryApiClient;
            LoadTransferSummaries();
        }

        public TransfersSummaryApiClient TransferSummaryApiClient { get; set; }
        public ICommand DeleteCommand => new Command<TransferSummary>(DeleteTransferSummary);
        public ICommand EditCommand => new Command<TransferSummary>(EditTransferSummary);
        public ICommand RefreshCommand => new Command(async () => await LoadTransferSummaries());
        public ICommand SearchCommand => new Command(async () => await FilterTransferSummaryAsync());
        public ICommand CreateCommand => new Command(async () => await Shell.Current.GoToAsync("CreateTransferSummary"));

        private List<TransferSummary> _allTransferSummaries;
        public List<TransferSummary> AllTransferSummaries
        {
            get => _allTransferSummaries;
            set
            {
                _allTransferSummaries = value;
                OnPropertyChanged(nameof(AllTransferSummaries));
            }
        }

        private List<TransferSummary> _transferSummaries;
        public List<TransferSummary> TransferSummaries
        {
            get => _transferSummaries;
            set
            {
                _transferSummaries = value;
                OnPropertyChanged(nameof(TransferSummaries));
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

        public async Task LoadTransferSummaries()
        {
            IsRefreshing = true;
            AllTransferSummaries = await TransferSummaryApiClient.GetTransferSummariesAsync();
            FilterTransferSummaries();
            IsRefreshing = false;
        }

        public async void DeleteTransferSummary(TransferSummary transferSummary)
        {
            transferSummary.IsActive = false;
            bool response = await TransferSummaryApiClient.UpdateTransferSummaryAsync(transferSummary);
            if (response) ThrowMessage.ShowSuccessMessage("Resumen de transferencia eliminado correctamente");
            else ThrowMessage.ShowErrorMessage("Error al eliminar el resumen de transferencia");
            await LoadTransferSummaries();
        }

        public async void EditTransferSummary(TransferSummary transferSummary)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "TransferSummary" , transferSummary }
            };

            await Shell.Current.GoToAsync("EditTransferSummary", parameters);
        }

        public async Task FilterTransferSummaryAsync()
        {
            try
            {
                FilterTransferSummaries();
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to filter transfer summaries: " + ex.Message);
            }
        }

        private void FilterTransferSummaries()
        {
            var filteredTransferSummaries = AllTransferSummaries;

            // Filtrar resúmenes de transferencias por el texto de búsqueda en Año, Total Ingresos, Total Gastos, etc.
            if (!string.IsNullOrEmpty(SearchText))
            {
                filteredTransferSummaries = filteredTransferSummaries.Where(ts =>
                    (ts.Year.HasValue && ts.Year.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (ts.TotalIncome.HasValue && ts.TotalIncome.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (ts.TotalExpenses.HasValue && ts.TotalExpenses.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (ts.NetIncome.HasValue && ts.NetIncome.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (ts.RetainedEarning.HasValue && ts.RetainedEarning.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (ts.BankBox.HasValue && ts.BankBox.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            if (SeeInactives)
            {
                TransferSummaries = filteredTransferSummaries.Where(ts => ts.IsActive == false).ToList();
            }
            else
            {
                TransferSummaries = filteredTransferSummaries.Where(ts => ts.IsActive == true).ToList();
            }
        }
    }
}
