using HFPMapp.ViewModels.Accounting.PettyCashSummary;

namespace HFPMapp.Views.Accounting.PettyCashSummary;

public partial class ListView : ContentPage
{
	public ListView(ListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}