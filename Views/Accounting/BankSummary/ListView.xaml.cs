using HFPMapp.ViewModels.Accounting.BankSummary;

namespace HFPMapp.Views.Accounting.BankSummary;

public partial class ListView : ContentPage
{
	public ListView(ListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}