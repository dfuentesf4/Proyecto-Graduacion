using HFPMapp.ViewModels.Accounting.Bank;

namespace HFPMapp.Views.Accounting.Bank;

public partial class EditView : ContentPage
{
	public EditView(EditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}