using HFPMapp.Models;
using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Accounting.RevenuesDetails
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel(RevenuesDetailApiClient revenuesDetailApiClient)
        {
            RevenuesDetailApiClient = revenuesDetailApiClient;
            LoadRevenuesDetails();
        }

        public RevenuesDetailApiClient RevenuesDetailApiClient { get; set; }
        public ICommand DeleteCommand => new Command<RevenuesDetail>(DeleteRevenuesDetail);
        public ICommand EditCommand => new Command<RevenuesDetail>(EditRevenuesDetail);
        public ICommand RefreshCommand => new Command(async () => await LoadRevenuesDetails());
        public ICommand SearchCommand => new Command(async () => await FilterRevenuesDetailAsync());
        public ICommand CreateCommand => new Command(async () => await Shell.Current.GoToAsync("CreateRevenuesDetail"));

        private List<RevenuesDetail> _allRevenuesDetails;
        public List<RevenuesDetail> AllRevenuesDetails
        {
            get => _allRevenuesDetails;
            set
            {
                _allRevenuesDetails = value;
                OnPropertyChanged(nameof(AllRevenuesDetails));
            }
        }

        private List<RevenuesDetail> _revenuesDetails;
        public List<RevenuesDetail> RevenuesDetails
        {
            get => _revenuesDetails;
            set
            {
                _revenuesDetails = value;
                OnPropertyChanged(nameof(RevenuesDetails));
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

        public async Task LoadRevenuesDetails()
        {
            IsRefreshing = true;
            AllRevenuesDetails = await RevenuesDetailApiClient.GetRevenuesDetailsAsync();
            FilterRevenuesDetails();
            IsRefreshing = false;
        }

        public async void DeleteRevenuesDetail(RevenuesDetail revenuesDetail)
        {
            revenuesDetail.IsActive = false;
            bool response = await RevenuesDetailApiClient.UpdateRevenuesDetailAsync(revenuesDetail);
            if (response) ThrowMessage.ShowSuccessMessage("Detalle de ingreso eliminado correctamente");
            else ThrowMessage.ShowErrorMessage("Error al eliminar el detalle de ingreso");
            await LoadRevenuesDetails();
        }

        public async void EditRevenuesDetail(RevenuesDetail revenuesDetail)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "RevenuesDetail" , revenuesDetail }
            };

            await Shell.Current.GoToAsync("EditRevenuesDetail", parameters);
        }

        public async Task FilterRevenuesDetailAsync()
        {
            try
            {
                FilterRevenuesDetails();
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to filter revenues details: " + ex.Message);
            }
        }

        private void FilterRevenuesDetails()
        {
            var filteredRevenuesDetails = AllRevenuesDetails;

            // Filtrar detalles de ingresos por el texto de búsqueda en Año, Mes, o Carpeta
            if (!string.IsNullOrEmpty(SearchText))
            {
                filteredRevenuesDetails = filteredRevenuesDetails.Where(rd =>
                    (rd.Year.HasValue && rd.Year.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (rd.Month.HasValue && rd.Month.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (rd.Folder.HasValue && rd.Folder.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            if (SeeInactives)
            {
                RevenuesDetails = filteredRevenuesDetails.Where(rd => rd.IsActive == false).ToList();
            }
            else
            {
                RevenuesDetails = filteredRevenuesDetails.Where(rd => rd.IsActive == true).ToList();
            }
        }
    }
}
