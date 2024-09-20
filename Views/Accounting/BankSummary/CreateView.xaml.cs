using HFPMapp.ViewModels.Accounting.BankSummary;

namespace HFPMapp.Views.Accounting.BankSummary;

public partial class CreateView : ContentPage
{
	public CreateView(CreateViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}