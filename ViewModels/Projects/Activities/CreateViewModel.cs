using HFPMapp.Models;
using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Projects.Activities
{
    public class CreateViewModel : BaseViewModel
    {
        public CreateViewModel(ActivityApiClient activityApiClient, ProjectApiClient projectApiClient)
        {
            ActivityApiClient = activityApiClient;
            ProjectApiClient = projectApiClient;
            CreateCommand = new Command(async () => await CreateActivity());
            _activity = new();
            LoadProjects();
        }

        public ActivityApiClient ActivityApiClient { get; set; }
        public ProjectApiClient ProjectApiClient { get; set; }
        public ICommand CreateCommand { get; }

        private Activity _activity;
        public Activity Activity
        {
            get => _activity;
            set
            {
                _activity = value;
                OnPropertyChanged(nameof(Activity));
            }
        }

        private List<Project> _projects;
        public List<Project> Projects
        {
            get => _projects;
            set
            {
                _projects = value;
                OnPropertyChanged(nameof(Projects));
            }
        }

        private Project _selectedProject;
        public Project SelectedProject
        {
            get => _selectedProject;
            set
            {
                _selectedProject = value;
                Activity.ProjectId = value?.Id;
                //Activity.Project = value;
                OnPropertyChanged(nameof(SelectedProject));
            }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public async Task LoadProjects()
        {
            Projects = await ProjectApiClient.GetProjectsAsync();
        }

        public bool ValidateModel()
        {
            var context = new ValidationContext(Activity, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(Activity, context, results, true);

            if (!isValid)
            {
                ErrorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            }

            return isValid;
        }

        public void ClearFields()
        {
            Activity = new();
        }

        private async Task CreateActivity()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isCreated = await ActivityApiClient.CreateActivityAsync(Activity);

            if (isCreated)
            {
                ThrowMessage.ShowSuccessMessage("Actividad Creada con éxito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error creando la Actividad");
            }

            IsBusy = false;
        }
    }
}
