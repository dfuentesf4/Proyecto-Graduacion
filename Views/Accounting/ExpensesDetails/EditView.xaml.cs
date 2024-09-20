using HFPMapp.ViewModels.Accounting.ExpensesDetails;

namespace HFPMapp.Views.Accounting.ExpensesDetails;

public partial class EditView : ContentPage
{
	public EditView(EditViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}