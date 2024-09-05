using HFPMapp.ViewModels.Projects.Activities;

namespace HFPMapp.Views.Projects.Activities;

public partial class CreateView : ContentPage
{
	public CreateView(CreateViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}