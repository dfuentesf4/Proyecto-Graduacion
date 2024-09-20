using HFPMapp.ViewModels.Accounting.TransfersSummary;

namespace HFPMapp.Views.Accounting.TransfersSummary;

public partial class EditView : ContentPage
{
	public EditView(EditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}