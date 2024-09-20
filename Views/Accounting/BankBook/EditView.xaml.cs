using HFPMapp.ViewModels.Accounting.BankBook;

namespace HFPMapp.Views.Accounting.BankBook;

public partial class EditView : ContentPage
{
	public EditView(EditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}