using HFPMapp.Models;
using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Projects.Beneficiaries
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel(BeneficiaryApiClient beneficiaryApiClient)
        {
            BeneficiaryApiClient = beneficiaryApiClient;
            LoadBeneficiaries();
        }

        public BeneficiaryApiClient BeneficiaryApiClient { get; set; }
        public ICommand DeleteCommand => new Command<Beneficiary>(DeleteBeneficiary);
        public ICommand EditCommand => new Command<Beneficiary>(EditBeneficiary);
        public ICommand RefreshCommand => new Command(async () => await LoadBeneficiaries());
        public ICommand SearchCommand => new Command(async () => await FilterBeneficiaryAsync());
        public ICommand CreateCommand => new Command(async () => await Shell.Current.GoToAsync("CreateBeneficiary"));

        private List<Beneficiary> _AllBeneficiaries;
        public List<Beneficiary> AllBeneficiaries
        {
            get
            {
                return _AllBeneficiaries;
            }
            set
            {
                _AllBeneficiaries = value;
                OnPropertyChanged(nameof(AllBeneficiaries));
            }
        }

        private List<Beneficiary> _beneficiaries;
        public List<Beneficiary> Beneficiaries
        {
            get
            {
                return _beneficiaries;
            }
            set
            {
                _beneficiaries = value;
                OnPropertyChanged(nameof(Beneficiaries));
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

        private string _SearchText;
        public string SearchText
        {
            get
            {
                return _SearchText;
            }
            set
            {
                _SearchText = value;
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

        public async Task LoadBeneficiaries()
        {
            IsRefreshing = true;
            AllBeneficiaries = await BeneficiaryApiClient.GetBeneficiariesAsync();
            if (SeeInactives) Beneficiaries = AllBeneficiaries.Where(b => b.IsActive == false).ToList();
            else Beneficiaries = AllBeneficiaries.Where(b => b.IsActive == true).ToList();
            IsRefreshing = false;
        }

        public async void DeleteBeneficiary(Beneficiary beneficiary)
        {
            beneficiary.IsActive = false;
            bool response = await BeneficiaryApiClient.UpdateBeneficiaryAsync(beneficiary);
            if (response) ThrowMessage.ShowSuccessMessage("Beneficiario eliminado correctamente");
            else ThrowMessage.ShowErrorMessage("Error al eliminar el Beneficiario");
            await LoadBeneficiaries();
        }

        public async void EditBeneficiary(Beneficiary beneficiary)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "Beneficiary" , beneficiary }
            };

            await Shell.Current.GoToAsync($"EditBeneficiary", parameters);
        }

        public async Task FilterBeneficiaryAsync()
        {
            try
            {
                // Obtener todos los beneficiarios
                List<Beneficiary> allBeneficiaries = AllBeneficiaries;

                // Filtrar beneficiarios por el texto de búsqueda en FirstName, LastName, Email o PhoneNumber
                var filteredBeneficiaries = allBeneficiaries.Where(b =>
                    (!string.IsNullOrEmpty(b.Name) && b.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) 
                ).ToList();

                if (SeeInactives) Beneficiaries = filteredBeneficiaries.Where(b => b.IsActive == false).ToList();
                else Beneficiaries = filteredBeneficiaries.Where(b => b.IsActive == true).ToList();

            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to filter beneficiaries: " + ex.Message);
            }
        }
    }
}
