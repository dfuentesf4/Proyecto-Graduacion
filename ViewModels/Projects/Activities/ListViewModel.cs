using HFPMapp.Models;
using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Projects.Activities
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel(ActivityApiClient activityApiClient)
        {
            ActivityApiClient = activityApiClient;
            LoadActivities();
        }

        public ActivityApiClient ActivityApiClient { get; set; }
        public ICommand DeleteCommand => new Command<Activity>(DeleteActivity);
        public ICommand EditCommand => new Command<Activity>(EditActivity);
        public ICommand RefreshCommand => new Command(async () => await LoadActivities());
        public ICommand SearchCommand => new Command(async () => await FilterActivityAsync());
        public ICommand CreateCommand => new Command(async () => await Shell.Current.GoToAsync("CreateActivity"));

        private List<Activity> _allActivities;
        public List<Activity> AllActivities
        {
            get => _allActivities;
            set
            {
                _allActivities = value;
                OnPropertyChanged(nameof(AllActivities));
            }
        }

        private List<Activity> _activities;
        public List<Activity> Activities
        {
            get => _activities;
            set
            {
                _activities = value;
                OnPropertyChanged(nameof(Activities));
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

        public async Task LoadActivities()
        {
            IsRefreshing = true;
            AllActivities = await ActivityApiClient.GetActivitiesAsync();
            if (SeeInactives)
                Activities = AllActivities.Where(a => a.IsActive == false).ToList();
            else
                Activities = AllActivities.Where(a => a.IsActive == true).ToList();
            IsRefreshing = false;
        }

        public async void DeleteActivity(Activity activity)
        {
            activity.IsActive = false;
            bool response = await ActivityApiClient.UpdateActivityAsync(activity);
            if (response)
                ThrowMessage.ShowSuccessMessage("Actividad eliminada correctamente");
            else
                ThrowMessage.ShowErrorMessage("Error al eliminar la Actividad");
            await LoadActivities();
        }

        public async void EditActivity(Activity activity)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "Activity" , activity }
            };

            await Shell.Current.GoToAsync("EditActivity", parameters);
        }

        public async Task FilterActivityAsync()
        {
            try
            {
                // Obtener todas las actividades
                List<Activity> allActivities = AllActivities;

                // Filtrar actividades por el texto de búsqueda en Name o Description
                var filteredActivities = allActivities.Where(a =>
                    (!string.IsNullOrEmpty(a.Name) && a.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(a.Description) && a.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                ).ToList();

                if (SeeInactives)
                    Activities = filteredActivities.Where(a => a.IsActive == false).ToList();
                else
                    Activities = filteredActivities.Where(a => a.IsActive == true).ToList();

            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to filter activities: " + ex.Message);
            }
        }
    }
}
