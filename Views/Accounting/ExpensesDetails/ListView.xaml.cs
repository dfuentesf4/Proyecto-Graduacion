using HFPMapp.ViewModels.Accounting.ExpensesDetails;

namespace HFPMapp.Views.Accounting.ExpensesDetails;

public partial class ListView : ContentPage
{
	public ListView(ListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}