using HFPMapp.Models;
using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Accounting.TransfersFromUS
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel(TransfersFromUApiClient transfersFromUApiClient)
        {
            TransfersFromUApiClient = transfersFromUApiClient;
            LoadTransfersFromU();
        }

        public TransfersFromUApiClient TransfersFromUApiClient { get; set; }
        public ICommand DeleteCommand => new Command<TransfersFromU>(DeleteTransfer);
        public ICommand EditCommand => new Command<TransfersFromU>(EditTransfer);
        public ICommand RefreshCommand => new Command(async () => await LoadTransfersFromU());
        public ICommand SearchCommand => new Command(async () => await FilterTransfersFromUAsync());
        public ICommand CreateCommand => new Command(async () => await Shell.Current.GoToAsync("CreateTransfersFromU"));

        private List<TransfersFromU> _allTransfersFromU;
        public List<TransfersFromU> AllTransfersFromU
        {
            get => _allTransfersFromU;
            set
            {
                _allTransfersFromU = value;
                OnPropertyChanged(nameof(AllTransfersFromU));
            }
        }

        private List<TransfersFromU> _transfersFromU;
        public List<TransfersFromU> TransfersFromU
        {
            get => _transfersFromU;
            set
            {
                _transfersFromU = value;
                OnPropertyChanged(nameof(TransfersFromU));
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
                FilterTransfersFromU(); // Filter list when toggling 'See Inactives'
            }
        }

        public async Task LoadTransfersFromU()
        {
            IsRefreshing = true;
            AllTransfersFromU = await TransfersFromUApiClient.GetTransfersFromUAsync();
            FilterTransfersFromU();
            IsRefreshing = false;
        }

        public async void DeleteTransfer(TransfersFromU transfer)
        {
            transfer.IsActive = false;
            bool response = await TransfersFromUApiClient.UpdateTransfersFromUAsync(transfer);
            if (response)
                ThrowMessage.ShowSuccessMessage("Transferencia eliminada correctamente");
            else
                ThrowMessage.ShowErrorMessage("Error al eliminar la transferencia");

            await LoadTransfersFromU();
        }

        public async void EditTransfer(TransfersFromU transfer)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "TransfersFromU" , transfer }
            };

            await Shell.Current.GoToAsync("EditTransfersFromU", parameters);
        }

        public async Task FilterTransfersFromUAsync()
        {
            try
            {
                FilterTransfersFromU();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to filter transfers: " + ex.Message);
            }
        }

        private void FilterTransfersFromU()
        {
            var filteredTransfers = AllTransfersFromU;

            // Filtrar transferencias por el texto de búsqueda en Fecha, Carpeta, o Monto
            if (!string.IsNullOrEmpty(SearchText))
            {
                filteredTransfers = filteredTransfers.Where(t =>
                    (t.Date.HasValue && t.Date.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(t.Folder) && t.Folder.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (t.Amount.HasValue && t.Amount.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            if (SeeInactives)
            {
                TransfersFromU = filteredTransfers.Where(t => t.IsActive == false).ToList();
            }
            else
            {
                TransfersFromU = filteredTransfers.Where(t => t.IsActive == true).ToList();
            }
        }
    }
}
