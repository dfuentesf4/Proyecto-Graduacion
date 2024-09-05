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
    [QueryProperty(nameof(Activity), "Activity")]
    public class EditViewModel : BaseViewModel
    {
        public EditViewModel(ActivityApiClient activityApiClient, ProjectApiClient projectApiClient)
        {
            ActivityApiClient = activityApiClient;
            ProjectApiClient = projectApiClient;
            LoadProjects();
        }

        public ActivityApiClient ActivityApiClient { get; set; }
        public ProjectApiClient ProjectApiClient { get; set; }
        public ICommand EditCommand => new Command(EditActivity);

        private Activity _activity;
        public Activity Activity
        {
            get => _activity;
            set
            {
                _activity = value;
                if (_projects != null && _activity.ProjectId.HasValue)
                {
                    SelectedProject = _projects.FirstOrDefault(p => p.Id == _activity.ProjectId);
                }
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
                // Establece el proyecto seleccionado basado en el Activity cuando los proyectos estén disponibles
                if (_activity != null && _activity.ProjectId.HasValue)
                {
                    SelectedProject = _projects.FirstOrDefault(p => p.Id == _activity.ProjectId);
                }
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
                if (value != null)
                {
                    Activity.ProjectId = value.Id;
                    Activity.Project = value;
                }
                OnPropertyChanged(nameof(SelectedProject));
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
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

            if (Activity != null && Activity.ProjectId.HasValue)
            {
                SelectedProject = Projects.FirstOrDefault(p => p.Id == Activity.ProjectId);
            }
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

        public async void EditActivity()
        {
            IsBusy = true;
            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isEdited = await ActivityApiClient.UpdateActivityAsync(Activity);

            if (isEdited)
            {
                ThrowMessage.ShowSuccessMessage("Actividad Editada con éxito");
                await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error editando la Actividad");
            }
            IsBusy = false;
        }
    }
}
