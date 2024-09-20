using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Accounting.BankSummary
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel(BankSummaryApiClient bankSummaryApiClient)
        {
            BankSummaryApiClient = bankSummaryApiClient;
            LoadBankSummaries();
        }

        public BankSummaryApiClient BankSummaryApiClient { get; set; }
        public ICommand DeleteCommand => new Command<HFPMapp.Models.BankSummary>(DeleteBankSummary);
        public ICommand EditCommand => new Command<HFPMapp.Models.BankSummary>(EditBankSummary);
        public ICommand RefreshCommand => new Command(async () => await LoadBankSummaries());
        public ICommand SearchCommand => new Command(async () => await FilterBankSummariesAsync());
        public ICommand CreateCommand => new Command(async () => await Shell.Current.GoToAsync("CreateBankSummary"));

        private List<HFPMapp.Models.BankSummary> _allBankSummaries;
        public List<HFPMapp.Models.BankSummary> AllBankSummaries
        {
            get => _allBankSummaries;
            set
            {
                _allBankSummaries = value;
                OnPropertyChanged(nameof(AllBankSummaries));
            }
        }

        private List<HFPMapp.Models.BankSummary> _bankSummaries;
        public List<HFPMapp.Models.BankSummary> BankSummaries
        {
            get => _bankSummaries;
            set
            {
                _bankSummaries = value;
                OnPropertyChanged(nameof(BankSummaries));
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

        public async Task LoadBankSummaries()
        {
            IsRefreshing = true;
            AllBankSummaries = await BankSummaryApiClient.GetBankSummariesAsync();
            FilterBankSummaries();
            IsRefreshing = false;
        }

        public async void DeleteBankSummary(HFPMapp.Models.BankSummary bankSummary)
        {
            bankSummary.IsActive = false;
            bool response = await BankSummaryApiClient.UpdateBankSummaryAsync(bankSummary);
            if (response)
            {
                ThrowMessage.ShowSuccessMessage("Resumen bancario eliminado correctamente");
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Error al eliminar el resumen bancario");
            }
            await LoadBankSummaries();
        }

        public async void EditBankSummary(HFPMapp.Models.BankSummary bankSummary)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "BankSummary" , bankSummary }
            };

            await Shell.Current.GoToAsync("EditBankSummary", parameters);
        }

        public async Task FilterBankSummariesAsync()
        {
            try
            {
                FilterBankSummaries();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to filter bank summaries: " + ex.Message);
            }
        }

        private void FilterBankSummaries()
        {
            var filteredBankSummaries = AllBankSummaries;

            if (!string.IsNullOrEmpty(SearchText))
            {
                filteredBankSummaries = filteredBankSummaries.Where(bs =>
                    (bs.Year.HasValue && bs.Year.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (bs.Month.HasValue && bs.Month.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(bs.Bank?.Name) && bs.Bank.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            if (SeeInactives)
            {
                BankSummaries = filteredBankSummaries.Where(bs => bs.IsActive == false).ToList();
            }
            else
            {
                BankSummaries = filteredBankSummaries.Where(bs => bs.IsActive == true).ToList();
            }
        }
    }
}
