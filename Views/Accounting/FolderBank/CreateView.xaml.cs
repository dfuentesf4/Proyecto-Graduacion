using HFPMapp.ViewModels.Accounting.FolderBank;

namespace HFPMapp.Views.Accounting.FolderBank;

public partial class CreateView : ContentPage
{
	public CreateView(CreateViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}