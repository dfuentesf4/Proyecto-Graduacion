using HFPMapp.ViewModels.Accounting.Bank;

namespace HFPMapp.Views.Accounting.Bank;

public partial class ListView : ContentPage
{
	public ListView(ListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}