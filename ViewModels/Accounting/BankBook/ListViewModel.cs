using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Accounting.BankBook
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel(BankBookApiClient bankBookApiClient)
        {
            BankBookApiClient = bankBookApiClient;
            LoadBankBooks();
        }

        public BankBookApiClient BankBookApiClient { get; set; }
        public ICommand DeleteCommand => new Command<HFPMapp.Models.BankBook>(DeleteBankBook);
        public ICommand EditCommand => new Command<HFPMapp.Models.BankBook>(EditBankBook);
        public ICommand RefreshCommand => new Command(async () => await LoadBankBooks());
        public ICommand SearchCommand => new Command(async () => await FilterBankBooksAsync());
        public ICommand CreateCommand => new Command(async () => await Shell.Current.GoToAsync("CreateBankBook"));

        private List<HFPMapp.Models.BankBook> _allBankBooks;
        public List<HFPMapp.Models.BankBook> AllBankBooks
        {
            get => _allBankBooks;
            set
            {
                _allBankBooks = value;
                OnPropertyChanged(nameof(AllBankBooks));
            }
        }

        private List<HFPMapp.Models.BankBook> _bankBooks;
        public List<HFPMapp.Models.BankBook> BankBooks
        {
            get => _bankBooks;
            set
            {
                _bankBooks = value;
                OnPropertyChanged(nameof(BankBooks));
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

        public async Task LoadBankBooks()
        {
            IsRefreshing = true;
            AllBankBooks = await BankBookApiClient.GetBankBooksAsync();
            FilterBankBooks();
            IsRefreshing = false;
        }

        public async void DeleteBankBook(HFPMapp.Models.BankBook bankBook)
        {
            bankBook.IsActive = false;
            bool response = await BankBookApiClient.UpdateBankBookAsync(bankBook);
            if (response)
            {
                ThrowMessage.ShowSuccessMessage("Libro bancario eliminado correctamente");
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Error al eliminar el libro bancario");
            }
            await LoadBankBooks();
        }

        public async void EditBankBook(HFPMapp.Models.BankBook bankBook)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "BankBook", bankBook }
            };

            await Shell.Current.GoToAsync("EditBankBook", parameters);
        }

        public async Task FilterBankBooksAsync()
        {
            try
            {
                FilterBankBooks();
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to filter bank books: " + ex.Message);
            }
        }

        private void FilterBankBooks()
        {
            var filteredBankBooks = AllBankBooks;

            // Filtrar libros bancarios por el texto de búsqueda en varios campos
            if (!string.IsNullOrEmpty(SearchText))
            {
                filteredBankBooks = filteredBankBooks.Where(bb =>
                    (!string.IsNullOrEmpty(bb.PayrollNumber) && bb.PayrollNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(bb.Beneficiarie) && bb.Beneficiarie.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (bb.Amount.HasValue && bb.Amount.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            if (SeeInactives)
            {
                BankBooks = filteredBankBooks.Where(bb => bb.IsActive == false).ToList();
            }
            else
            {
                BankBooks = filteredBankBooks.Where(bb => bb.IsActive == true).ToList();
            }
        }
    }
}
