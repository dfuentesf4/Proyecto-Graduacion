using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Accounting.Bank
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel(BankApiClient bankApiClient)
        {
            BankApiClient = bankApiClient;
            LoadBanks();
        }

        public BankApiClient BankApiClient { get; set; }
        public ICommand DeleteCommand => new Command<HFPMapp.Models.Bank>(DeleteBank);
        public ICommand EditCommand => new Command<HFPMapp.Models.Bank>(EditBank);
        public ICommand RefreshCommand => new Command(async () => await LoadBanks());
        public ICommand SearchCommand => new Command(async () => await FilterBanksAsync());
        public ICommand CreateCommand => new Command(async () => await Shell.Current.GoToAsync("CreateBank"));

        private List<HFPMapp.Models.Bank> _allBanks;
        public List<HFPMapp.Models.Bank> AllBanks
        {
            get => _allBanks;
            set
            {
                _allBanks = value;
                OnPropertyChanged(nameof(AllBanks));
            }
        }

        private List<HFPMapp.Models.Bank> _banks;
        public List<HFPMapp.Models.Bank> Banks
        {
            get => _banks;
            set
            {
                _banks = value;
                OnPropertyChanged(nameof(Banks));
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

        public async Task LoadBanks()
        {
            IsRefreshing = true;
            AllBanks = await BankApiClient.GetBanksAsync();
            FilterBanks();
            IsRefreshing = false;
        }

        public async void DeleteBank(HFPMapp.Models.Bank bank)
        {
            bank.IsActive = false;
            bool response = await BankApiClient.UpdateBankAsync(bank);
            if (response) ThrowMessage.ShowSuccessMessage("Banco eliminado correctamente");
            else ThrowMessage.ShowErrorMessage("Error al eliminar el banco");
            await LoadBanks();
        }

        public async void EditBank(HFPMapp.Models.Bank bank)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "Bank", bank }
            };

            await Shell.Current.GoToAsync("EditBank", parameters);
        }

        public async Task FilterBanksAsync()
        {
            try
            {
                FilterBanks();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to filter banks: " + ex.Message);
            }
        }

        private void FilterBanks()
        {
            var filteredBanks = AllBanks;

            // Filtrar bancos por el texto de búsqueda en Nombre
            if (!string.IsNullOrEmpty(SearchText))
            {
                filteredBanks = filteredBanks.Where(b =>
                    (!string.IsNullOrEmpty(b.Name) && b.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            // Filtrar por bancos inactivos
            if (SeeInactives)
            {
                Banks = filteredBanks.Where(b => b.IsActive == false).ToList();
            }
            else
            {
                Banks = filteredBanks.Where(b => b.IsActive == true).ToList();
            }
        }
    }
}
