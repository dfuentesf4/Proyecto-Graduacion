using HFPMapp.Models;
using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Accounting.ExpensesDetails
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel(ExpensesDetailApiClient expensesDetailApiClient)
        {
            ExpensesDetailApiClient = expensesDetailApiClient;
            LoadExpensesDetails();
        }

        public ExpensesDetailApiClient ExpensesDetailApiClient { get; set; }
        public ICommand DeleteCommand => new Command<ExpensesDetail>(DeleteExpensesDetail);
        public ICommand EditCommand => new Command<ExpensesDetail>(EditExpensesDetail);
        public ICommand RefreshCommand => new Command(async () => await LoadExpensesDetails());
        public ICommand SearchCommand => new Command(async () => await FilterExpensesDetailAsync());
        public ICommand CreateCommand => new Command(async () => await Shell.Current.GoToAsync("CreateExpensesDetail"));

        private List<ExpensesDetail> _allExpensesDetails;
        public List<ExpensesDetail> AllExpensesDetails
        {
            get => _allExpensesDetails;
            set
            {
                _allExpensesDetails = value;
                OnPropertyChanged(nameof(AllExpensesDetails));
            }
        }

        private List<ExpensesDetail> _expensesDetails;
        public List<ExpensesDetail> ExpensesDetails
        {
            get => _expensesDetails;
            set
            {
                _expensesDetails = value;
                OnPropertyChanged(nameof(ExpensesDetails));
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

        public async Task LoadExpensesDetails()
        {
            IsRefreshing = true;
            AllExpensesDetails = await ExpensesDetailApiClient.GetExpensesDetailsAsync();
            FilterExpensesDetails();
            IsRefreshing = false;
        }

        public async void DeleteExpensesDetail(ExpensesDetail expensesDetail)
        {
            expensesDetail.IsActive = false;
            bool response = await ExpensesDetailApiClient.UpdateExpensesDetailAsync(expensesDetail);
            if (response) ThrowMessage.ShowSuccessMessage("Detalle de gasto eliminado correctamente");
            else ThrowMessage.ShowErrorMessage("Error al eliminar el detalle de gasto");
            await LoadExpensesDetails();
        }

        public async void EditExpensesDetail(ExpensesDetail expensesDetail)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "ExpensesDetail" , expensesDetail }
            };

            await Shell.Current.GoToAsync("EditExpensesDetail", parameters);
        }

        public async Task FilterExpensesDetailAsync()
        {
            try
            {
                FilterExpensesDetails();
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to filter expenses details: " + ex.Message);
            }
        }

        private void FilterExpensesDetails()
        {
            var filteredExpensesDetails = AllExpensesDetails;

            // Filtrar detalles de gastos por el texto de búsqueda en Año, Mes, o Carpeta
            if (!string.IsNullOrEmpty(SearchText))
            {
                filteredExpensesDetails = filteredExpensesDetails.Where(ed =>
                    (ed.Year.HasValue && ed.Year.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (ed.Month.HasValue && ed.Month.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (ed.Folder.HasValue && ed.Folder.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            if (SeeInactives)
            {
                ExpensesDetails = filteredExpensesDetails.Where(ed => ed.IsActive == false).ToList();
            }
            else
            {
                ExpensesDetails = filteredExpensesDetails.Where(ed => ed.IsActive == true).ToList();
            }
        }
    }
}
