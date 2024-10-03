using HFPMapp.Services.HTTP;
using HFPMapp.Services.Reports.Chargers;
using HFPMapp.Services.Reports.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.ViewModels.Reports.Projects
{
    public class ProjectsViewModel : BaseViewModel
    {
        public ProjectsViewModel(ProjectApiClient projectApiClient)
        {
            _projectApiClient = projectApiClient;
            _projectsCharger = new ProjectsCharger(_projectApiClient);

            ColorModel = new ObservableCollection<Brush>()
            {
                new SolidColorBrush(Color.FromArgb("#08ff00")),
                new SolidColorBrush(Color.FromArgb("#ff0000")),
                new SolidColorBrush(Color.FromArgb("#74ff00")),
                new SolidColorBrush(Color.FromArgb("#00a6ff")),
                new SolidColorBrush(Color.FromArgb("#7c00ff")),
            };

            LoadProjects();
        }

        private ProjectApiClient _projectApiClient;
        private ProjectsCharger _projectsCharger;
        public List<ProjectsPerMonth> projects { get; set; }

        public ObservableCollection<Brush> ColorModel { get; set; }

        private async Task LoadProjects()
        {
            projects = await _projectsCharger.LoadProjectsPerMonth();
            projects = projects.OrderBy(p => p.Year).ThenBy(p => p.Month).ToList();

            

            OnPropertyChanged(nameof(projects));
        }

    }

}
