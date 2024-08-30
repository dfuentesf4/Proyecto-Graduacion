using HFPMapp.ViewModels.Users;

namespace HFPMapp.Views.Users;

public partial class CreateView : ContentPage
{
	public CreateView(CreateViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}