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

namespace HFPMapp.ViewModels.Projects.Beneficiaries
{
    [QueryProperty(nameof(Beneficiary), "Beneficiary")]
    public class EditViewModel : BaseViewModel
    {
        public EditViewModel(BeneficiaryApiClient beneficiaryApiClient, ProjectApiClient projectApiClient)
        {
            BeneficiaryApiClient = beneficiaryApiClient;
            ProjectApiClient = projectApiClient;
            LoadProjects();
        }

        public BeneficiaryApiClient BeneficiaryApiClient { get; set; }
        public ProjectApiClient ProjectApiClient { get; set; }
        public ICommand EditCommand => new Command(EditBeneficiary);

        private Beneficiary _beneficiary;
        public Beneficiary Beneficiary
        {
            get
            {
                return _beneficiary;
            }
            set
            {
                _beneficiary = value;
                OnPropertyChanged(nameof(Beneficiary));
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
                Beneficiary.ProjectId = value.Id;
                Beneficiary.Project = value;
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
        }

        public bool ValidateModel()
        {
            var context = new ValidationContext(Beneficiary, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(Beneficiary, context, results, true);

            if (!isValid)
            {
                ErrorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            }

            return isValid;
        }

        public async void EditBeneficiary()
        {
            IsBusy = true;
            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isEdited = await BeneficiaryApiClient.UpdateBeneficiaryAsync(Beneficiary);

            if (isEdited)
            {
                ThrowMessage.ShowSuccessMessage("Beneficiario Editado con éxito");
                await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error editando el Beneficiario");
            }
            IsBusy = false;
        }
    }
}
