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

    [QueryProperty(nameof(Project), "Project")]
    public class ProjectEditViewModel : BaseViewModel
    {
        public ProjectEditViewModel(ProjectApiClient projectApiClient)
        {
            ProjectApiClient = projectApiClient;
        }

        public ProjectApiClient ProjectApiClient { get; set; }
        public ICommand EditCommand => new Command(EditProject);


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

        public async void EditProject()
        {
            IsBusy = true;
            bool isModelValid = ValidateModel();

            if (!isModelValid) return;

            bool isEdited = await ProjectApiClient.UpdateProjectAsync(Project);

            if (isEdited)
            {
                ThrowMessage.ShowSuccessMessage("Proyecto Editado con exito");
                await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error Editando el Proyecto");
            }
            IsBusy = false;
        }
    }
}
