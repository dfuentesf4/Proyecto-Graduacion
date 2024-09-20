using HFPMapp.ViewModels.Accounting.TransfersFromUS;

namespace HFPMapp.Views.Accounting.TransfersFromUS;

public partial class ListView : ContentPage
{
	public ListView(ListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}