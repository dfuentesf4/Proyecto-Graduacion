using HFPMapp.ViewModels.Donors;

namespace HFPMapp.Views.Donors;

public partial class ListView : ContentPage
{
	public ListView(ListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}