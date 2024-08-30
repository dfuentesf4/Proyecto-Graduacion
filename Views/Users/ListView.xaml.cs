using HFPMapp.ViewModels.Users;

namespace HFPMapp.Views.Users;

public partial class ListView : ContentPage
{
	public ListView(ListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}