using HFPMapp.ViewModels.Projects;

namespace HFPMapp.Views.Projects;

public partial class ProjectsListView : ContentPage
{
	public ProjectsListView(ProjectsListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;

    }
}