using HFPMapp.ViewModels.Donors;

namespace HFPMapp.Views.Donors;

public partial class CreateView : ContentPage
{
	public CreateView(CreateViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}