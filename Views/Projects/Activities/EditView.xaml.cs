using HFPMapp.ViewModels.Projects.Activities;

namespace HFPMapp.Views.Projects.Activities;

public partial class EditView : ContentPage
{
	public EditView(EditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}