using HFPMapp.ViewModels.Accounting.RevenuesDetails;

namespace HFPMapp.Views.Accounting.RevenuesDetails;

public partial class EditView : ContentPage
{
	public EditView(EditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}