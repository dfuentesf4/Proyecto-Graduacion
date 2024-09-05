using HFPMapp.ViewModels.Projects.Beneficiaries;

namespace HFPMapp.Views.Projects.Beneficiaries;

public partial class EditView : ContentPage
{
	public EditView(EditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}