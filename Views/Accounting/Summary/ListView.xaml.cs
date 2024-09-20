using HFPMapp.ViewModels.Accounting.Summary;

namespace HFPMapp.Views.Accounting.Summary;

public partial class ListView : ContentPage
{
	public ListView(ListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}