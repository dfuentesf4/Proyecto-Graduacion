using HFPMapp.ViewModels.Accounting.TransfersFromUS;

namespace HFPMapp.Views.Accounting.TransfersFromUS;

public partial class EditView : ContentPage
{
	public EditView(EditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}