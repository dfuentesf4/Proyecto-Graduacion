using HFPMapp.Models;
using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Projects.Volunteers
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel(VolunteerApiClient volunteerApiClient)
        {
            VolunteerApiClient = volunteerApiClient;
            LoadVolunteers();
        }

        public VolunteerApiClient VolunteerApiClient { get; set; }
        public ICommand DeleteCommand => new Command<Volunteer>(DeleteVolunteer);
        public ICommand EditCommand => new Command<Volunteer>(EditVolunteer);
        public ICommand RefreshCommand => new Command(async () => await LoadVolunteers());
        public ICommand SearchCommand => new Command(async () => await FilterVolunteerAsync());
        public ICommand CreateCommand => new Command(async () => await Shell.Current.GoToAsync("CreateVolunteer"));

        private List<Volunteer> _allVolunteers;
        public List<Volunteer> AllVolunteers
        {
            get
            {
                return _allVolunteers;
            }
            set
            {
                _allVolunteers = value;
                OnPropertyChanged(nameof(AllVolunteers));
            }
        }

        private List<Volunteer> _volunteers;
        public List<Volunteer> Volunteers
        {
            get
            {
                return _volunteers;
            }
            set
            {
                _volunteers = value;
                OnPropertyChanged(nameof(Volunteers));
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

        private string _searchText;
        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
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

        public async Task LoadVolunteers()
        {
            IsRefreshing = true;
            AllVolunteers = await VolunteerApiClient.GetVolunteersAsync();
            if (SeeInactives) Volunteers = AllVolunteers.Where(v => v.IsActive == false).ToList();
            else Volunteers = AllVolunteers.Where(v => v.IsActive == true).ToList();
            IsRefreshing = false;
        }

        public async void DeleteVolunteer(Volunteer volunteer)
        {
            volunteer.IsActive = false;
            bool response = await VolunteerApiClient.UpdateVolunteerAsync(volunteer);
            if (response) ThrowMessage.ShowSuccessMessage("Voluntario eliminado correctamente");
            else ThrowMessage.ShowErrorMessage("Error al eliminar el Voluntario");
            await LoadVolunteers();
        }

        public async void EditVolunteer(Volunteer volunteer)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "Volunteer" , volunteer }
            };

            await Shell.Current.GoToAsync($"EditVolunteer", parameters);
        }

        public async Task FilterVolunteerAsync()
        {
            try
            {
                // Obtener todos los voluntarios
                List<Volunteer> allVolunteers = AllVolunteers;

                // Filtrar voluntarios por el texto de búsqueda en FullName, Role, o PhoneNumber
                var filteredVolunteers = allVolunteers.Where(v =>
                    (!string.IsNullOrEmpty(v.FirstName) && v.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(v.LastName) && v.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(v.Role) && v.Role.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(v.PhoneNumber) && v.PhoneNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                ).ToList();

                if (SeeInactives) Volunteers = filteredVolunteers.Where(v => v.IsActive == false).ToList();
                else Volunteers = filteredVolunteers.Where(v => v.IsActive == true).ToList();

            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to filter volunteers: " + ex.Message);
            }
        }
    }
}
