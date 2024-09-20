using HFPMapp.ViewModels.Accounting.TransfersFromUS;

namespace HFPMapp.Views.Accounting.TransfersFromUS;

public partial class CreateView : ContentPage
{
	public CreateView(CreateViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}