using HFPMapp.ViewModels.Accounting.PettyCashSummary;

namespace HFPMapp.Views.Accounting.PettyCashSummary;

public partial class EditView : ContentPage
{
	public EditView(EditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}