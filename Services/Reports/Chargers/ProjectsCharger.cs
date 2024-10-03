using HFPMapp.Services.HTTP;
using HFPMapp.Services.Reports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Services.Reports.Chargers
{
    public class ProjectsCharger
    {
        public ProjectsCharger(ProjectApiClient projectApiClient)
        {
            _projectApiClient = projectApiClient;
        }

        private ProjectApiClient _projectApiClient;

        public async Task<List<ProjectsPerMonth>> LoadProjectsPerMonth()
        {
            var projects = await _projectApiClient.GetProjectsAsync();

            var projectsPerMonth = projects.Select(p => new ProjectsPerMonth
            {
                Month = p.StartDate.Value.Month,
                Year = p.StartDate.Value.Year,
                Name = p.Name,
                Budget = p.Budget.Value,
                StartDate = p.StartDate.Value,
                EndDate = p.EndDate.Value
            }).ToList();


            return projectsPerMonth;
        }
    }
}
