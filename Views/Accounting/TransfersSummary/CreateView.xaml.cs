using HFPMapp.ViewModels.Accounting.TransfersSummary;

namespace HFPMapp.Views.Accounting.TransfersSummary;

public partial class CreateView : ContentPage
{
	public CreateView(CreateViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}