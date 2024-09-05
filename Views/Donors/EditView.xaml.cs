using HFPMapp.ViewModels.Donors;

namespace HFPMapp.Views.Donors;

public partial class EditView : ContentPage
{
	public EditView(EditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}