using HFPMapp.Models;
using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Projects
{
    public class ProjectsListViewModel : BaseViewModel
    {
        public ProjectsListViewModel(ProjectApiClient projectApiClient)
        {
            ProjectApiClient = projectApiClient;
            LoadProjects();
        }

        public ProjectApiClient ProjectApiClient { get; set; }
        public ICommand DeleteCommand => new Command<Project>(DeleteProject);
        public ICommand EditCommand => new Command<Project>(EditProject);
        public ICommand RefreshCommand => new Command(async () => await LoadProjects());
        public ICommand SearchCommand => new Command(async () => await FilterProjectAsync());
        public ICommand CreateCommand => new Command(async () => await Shell.Current.GoToAsync("CreateProject"));

        private List<Project> _AllProjects;
        public List<Project> AllProjects
        {
            get
            {
                return _AllProjects;
            }
            set
            {
                _AllProjects = value;
                OnPropertyChanged(nameof(AllProjects));
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

        private string _SeachText;
        public string SearchText
        {
            get
            {
                return _SeachText;
            }
            set
            {
                _SeachText = value;
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

        public async Task LoadProjects()
        {
            IsRefreshing = true;
            AllProjects = await ProjectApiClient.GetProjectsAsync();
            if (SeeInactives) Projects = AllProjects.Where(u => u.IsActive == false).ToList();
            else Projects = AllProjects.Where(u => u.IsActive == true).ToList();
            IsRefreshing = false;
        }

        public async void DeleteProject(Project project)
        {
            project.IsActive = false;
            bool response = await ProjectApiClient.UpdateProjectAsync(project);
            if (response) ThrowMessage.ShowSuccessMessage("Donador eliminado correctamente");
            else ThrowMessage.ShowErrorMessage("Error al eliminar el Donador");
            await LoadProjects();
        }

        public async void EditProject(Project project)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "Project" , project }
            };

            await Shell.Current.GoToAsync($"EditProject", parameters);
        }

        public async Task FilterProjectAsync()
        {
            try
            {
                // Obtener todos los donadores
                List<Project> allProjects = AllProjects;

                // Filtrar donadores por el texto de búsqueda en FirstName, LastName, Email o PhoneNumber
                var filteredProjects = allProjects.Where(u =>
                    (!string.IsNullOrEmpty(u.Name) && u.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                ).ToList();

                if (SeeInactives) Projects = filteredProjects.Where(u => u.IsActive == false).ToList();
                else Projects = filteredProjects.Where(u => u.IsActive == true).ToList();

            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to filter Projects: " + ex.Message);
            }
        }
    }
}
