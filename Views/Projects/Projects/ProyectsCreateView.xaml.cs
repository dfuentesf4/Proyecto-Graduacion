using HFPMapp.ViewModels.Projects;

namespace HFPMapp.Views.Projects;

public partial class ProyectsCreateView : ContentPage
{
	public ProyectsCreateView(ProjectsCreateViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}