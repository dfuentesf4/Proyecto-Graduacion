using HFPMapp.ViewModels.Accounting.BankBook;

namespace HFPMapp.Views.Accounting.BankBook;

public partial class ListView : ContentPage
{
	public ListView(ListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}