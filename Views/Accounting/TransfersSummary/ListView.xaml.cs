using HFPMapp.ViewModels.Accounting.TransfersSummary;

namespace HFPMapp.Views.Accounting.TransfersSummary;

public partial class ListView : ContentPage
{
	public ListView(ListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}