using HFPMapp.ViewModels.Accounting.RevenuesDetails;

namespace HFPMapp.Views.Accounting.RevenuesDetails;

public partial class ListView : ContentPage
{
	public ListView(ListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}