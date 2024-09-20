using HFPMapp.ViewModels.Accounting.BankSummary;

namespace HFPMapp.Views.Accounting.BankSummary;

public partial class EditView : ContentPage
{
	public EditView(EditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}