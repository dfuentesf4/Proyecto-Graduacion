using HFPMapp.ViewModels.Users;

namespace HFPMapp.Views.Users;

public partial class EditView : ContentPage
{
	public EditView(EditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}