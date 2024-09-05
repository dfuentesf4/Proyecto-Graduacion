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

namespace HFPMapp.ViewModels.Projects.Volunteers
{
    public class CreateViewModel : BaseViewModel
    {
        public CreateViewModel(VolunteerApiClient volunteerApiClient, ProjectApiClient projectApiClient)
        {
            VolunteerApiClient = volunteerApiClient;
            ProjectApiClient = projectApiClient;
            CreateCommand = new Command(async () => await CreateVolunteer());
            _volunteer = new();
            LoadProjects();
        }

        public VolunteerApiClient VolunteerApiClient { get; set; }
        public ProjectApiClient ProjectApiClient { get; set; }
        public ICommand CreateCommand { get; }

        private Volunteer _volunteer;
        public Volunteer Volunteer
        {
            get
            {
                return _volunteer;
            }
            set
            {
                _volunteer = value;
                OnPropertyChanged(nameof(Volunteer));
            }
        }

        private List<Project> _projects;
        public List<Project> Projects
        {
            get
            {
                return _projects;
            }
            set
            {
                _projects = value;
                OnPropertyChanged(nameof(Projects));
            }
        }

        private Project _selectedProject;
        public Project SelectedProject
        {
            get
            {
                return _selectedProject;
            }
            set
            {
                _selectedProject = value;
                Volunteer.ProjectId = value.Id;
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
            get
            {
                return _isBusy;
            }
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
            var context = new ValidationContext(Volunteer, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(Volunteer, context, results, true);

            if (!isValid)
            {
                ErrorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            }

            return isValid;
        }

        public void ClearFields()
        {
            Volunteer = new();
        }

        private async Task CreateVolunteer()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isCreated = await VolunteerApiClient.CreateVolunteerAsync(Volunteer);

            if (isCreated)
            {
                ThrowMessage.ShowSuccessMessage("Voluntario Creado con éxito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error creando el Voluntario");
            }

            IsBusy = false;
        }
    }
}
