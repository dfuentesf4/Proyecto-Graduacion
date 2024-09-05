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

namespace HFPMapp.ViewModels.Projects
{
    public class ProjectsCreateViewModel : BaseViewModel
    {
        public ProjectsCreateViewModel(ProjectApiClient projectApiClient)
        {
            ProjectApiClient = projectApiClient;
            CreateCommand = new Command(async () => await CreateProject());
            _project = new()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today)
            };
        }

        public ProjectApiClient ProjectApiClient { get; set; }
        public ICommand CreateCommand { get; }


        private Project _project;
        public Project Project
        {
            get
            {
                return _project;
            }
            set
            {
                _project = value;
                OnPropertyChanged(nameof(Project));
            }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                _IsBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public bool ValidateModel()
        {
            var context = new ValidationContext(Project, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(Project, context, results, true);

            if (!isValid)
            {
                ErrorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            }

            return isValid;
        }

        public void ClearFields()
        {
            Project = new();
        }

        private async Task CreateProject()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isCreated = await ProjectApiClient.CreateProjectAsync(Project);

            if (isCreated)
            {
                ThrowMessage.ShowSuccessMessage("Proyecto Creado con exito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error creando el Proyecto");
            }

            IsBusy = false;
        }
    }
}
