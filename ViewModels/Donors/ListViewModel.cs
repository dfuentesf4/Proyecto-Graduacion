using HFPMapp.Models;
using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Donors
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel(DonorApiClient donorApiClient)
        {
            DonorApiClient = donorApiClient;
            LoadDonors();
        }

        public DonorApiClient DonorApiClient { get; set; }
        public ICommand DeleteCommand => new Command<Donor>(DeleteDonor);
        public ICommand EditCommand => new Command<Donor>(EditDonor);
        public ICommand RefreshCommand => new Command(async () => await LoadDonors());
        public ICommand SearchCommand => new Command(async () => await FilterDonorAsync());
        public ICommand CreateCommand => new Command(async () => await Shell.Current.GoToAsync("CreateDonor"));

        private List<Donor> _AllDonors;
        public List<Donor> AllDonors
        {
            get
            {
                return _AllDonors;
            }
            set
            {
                _AllDonors = value;
                OnPropertyChanged(nameof(AllDonors));
            }
        }

        private List<Donor> _donors;
        public List<Donor> Donors
        {
            get
            {
                return _donors;
            }
            set
            {
                _donors = value;
                OnPropertyChanged(nameof(Donors));
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        private string _SeachText;
        public string SearchText
        {
            get
            {
                return _SeachText;
            }
            set
            {
                _SeachText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private bool _seeInactives;
        public bool SeeInactives
        {
            get
            {
                return _seeInactives;
            }
            set
            {
                _seeInactives = value;
                OnPropertyChanged(nameof(SeeInactives));
            }
        }

        public async Task LoadDonors()
        {
            IsRefreshing = true;
            AllDonors = await DonorApiClient.GetDonorsAsync();
            if (SeeInactives) Donors = AllDonors.Where(u => u.IsActive == false).ToList();
            else Donors = AllDonors.Where(u => u.IsActive == true).ToList();
            IsRefreshing = false;
        }

        public async void DeleteDonor(Donor donor)
        {
            donor.IsActive = false;
            bool response = await DonorApiClient.UpdateDonorAsync(donor);
            if (response) ThrowMessage.ShowSuccessMessage("Donador eliminado correctamente");
            else ThrowMessage.ShowErrorMessage("Error al eliminar el Donador");
            await LoadDonors();
        }

        public async void EditDonor(Donor donor)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "Donor" , donor }
            };

            await Shell.Current.GoToAsync($"EditDonor", parameters);
        }

        public async Task FilterDonorAsync()
        {
            try
            {
                // Obtener todos los donadores
                List<Donor> allDonors = AllDonors;

                // Filtrar donadores por el texto de búsqueda en FirstName, LastName, Email o PhoneNumber
                var filteredDonors = allDonors.Where(u =>
                    (!string.IsNullOrEmpty(u.FirstName) && u.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(u.LastName) && u.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(u.PhoneNumber) && u.PhoneNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                ).ToList();

                if (SeeInactives) Donors = filteredDonors.Where(u => u.IsActive == false).ToList();
                else Donors = filteredDonors.Where(u => u.IsActive == true).ToList();

            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to filter donors: " + ex.Message);
            }
        }
    }
}
