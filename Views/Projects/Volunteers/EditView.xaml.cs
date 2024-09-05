using HFPMapp.ViewModels.Projects.Volunteers;

namespace HFPMapp.Views.Projects.Volunteers;

public partial class EditView : ContentPage
{
	public EditView(EditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}