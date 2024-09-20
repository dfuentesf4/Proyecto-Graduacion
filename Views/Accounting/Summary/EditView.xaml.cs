using HFPMapp.ViewModels.Accounting.Summary;

namespace HFPMapp.Views.Accounting.Summary;

public partial class EditView : ContentPage
{
	public EditView(EditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}