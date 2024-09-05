using HFPMapp.ViewModels.Projects;

namespace HFPMapp.Views.Projects;

public partial class ProjectEditView : ContentPage
{
	public ProjectEditView(ProjectEditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}