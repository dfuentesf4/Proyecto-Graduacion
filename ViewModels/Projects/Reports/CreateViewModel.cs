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

namespace HFPMapp.ViewModels.Projects.Reports
{
    public class CreateViewModel : BaseViewModel
    {
        public CreateViewModel(ReportApiClient reportApiClient, ProjectApiClient projectApiClient)
        {
            ReportApiClient = reportApiClient;
            ProjectApiClient = projectApiClient;
            CreateCommand = new Command(async () => await CreateReport());
            _report = new();
            LoadProjects();
        }

        public ReportApiClient ReportApiClient { get; set; }
        public ProjectApiClient ProjectApiClient { get; set; }
        public ICommand CreateCommand { get; }

        private Report _report;
        public Report Report
        {
            get
            {
                return _report;
            }
            set
            {
                _report = value;
                OnPropertyChanged(nameof(Report));
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
                Report.ProjectId = value.Id;
                //Report.Project = value;
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
            var context = new ValidationContext(Report, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(Report, context, results, true);

            if (!isValid)
            {
                ErrorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            }

            return isValid;
        }

        public void ClearFields()
        {
            Report = new();
        }

        private async Task CreateReport()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isCreated = await ReportApiClient.CreateReportAsync(Report);

            if (isCreated)
            {
                ThrowMessage.ShowSuccessMessage("Reporte creado con éxito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error creando el Reporte");
            }

            IsBusy = false;
        }
    }

}

