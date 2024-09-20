using HFPMapp.ViewModels.Accounting.FolderBank;

namespace HFPMapp.Views.Accounting.FolderBank;

public partial class ListView : ContentPage
{
	public ListView(ListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}