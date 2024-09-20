using HFPMapp.ViewModels.Accounting.ExpensesDetails;

namespace HFPMapp.Views.Accounting.ExpensesDetails;

public partial class CreateView : ContentPage
{
	public CreateView(CreateViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}