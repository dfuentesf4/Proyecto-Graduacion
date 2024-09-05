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
    [QueryProperty(nameof(Volunteer), "Volunteer")]
    public class EditViewModel : BaseViewModel
    {
        public EditViewModel(VolunteerApiClient volunteerApiClient, ProjectApiClient projectApiClient)
        {
            VolunteerApiClient = volunteerApiClient;
            ProjectApiClient = projectApiClient;
            LoadProjects();
        }

        public VolunteerApiClient VolunteerApiClient { get; set; }
        public ProjectApiClient ProjectApiClient { get; set; }
        public ICommand EditCommand => new Command(EditVolunteer);

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
                if (_projects != null && _volunteer.ProjectId.HasValue)
                {
                    SelectedProject = _projects.FirstOrDefault(p => p.Id == _volunteer.ProjectId);
                }
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
                // Establece el proyecto seleccionado basado en el Volunteer cuando los proyectos estén disponibles
                if (_volunteer != null && _volunteer.ProjectId.HasValue)
                {
                    SelectedProject = _projects.FirstOrDefault(p => p.Id == _volunteer.ProjectId);
                }
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
                if(value is not null) 
                {
                    Volunteer.ProjectId = value.Id;
                    Volunteer.Project = value;
                }
                OnPropertyChanged(nameof(SelectedProject));
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
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

            if (Volunteer != null && Volunteer.ProjectId.HasValue)
            {
                SelectedProject = Projects.FirstOrDefault(p => p.Id == Volunteer.ProjectId);
            }
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

        public async void EditVolunteer()
        {
            IsBusy = true;
            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isEdited = await VolunteerApiClient.UpdateVolunteerAsync(Volunteer);

            if (isEdited)
            {
                ThrowMessage.ShowSuccessMessage("Voluntario Editado con éxito");
                await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error editando el Voluntario");
            }
            IsBusy = false;
        }
    }
}
