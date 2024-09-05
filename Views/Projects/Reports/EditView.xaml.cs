using HFPMapp.ViewModels.Projects.Reports;

namespace HFPMapp.Views.Projects.Reports;

public partial class EditView : ContentPage
{
	public EditView(EditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}