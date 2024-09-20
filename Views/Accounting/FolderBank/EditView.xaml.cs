using HFPMapp.ViewModels.Accounting.FolderBank;

namespace HFPMapp.Views.Accounting.FolderBank;

public partial class EditView : ContentPage
{
	public EditView(EditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}