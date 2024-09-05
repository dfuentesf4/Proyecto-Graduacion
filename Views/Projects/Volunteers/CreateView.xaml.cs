using HFPMapp.ViewModels.Projects.Volunteers;

namespace HFPMapp.Views.Projects.Volunteers;

public partial class CreateView : ContentPage
{
	public CreateView(CreateViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}