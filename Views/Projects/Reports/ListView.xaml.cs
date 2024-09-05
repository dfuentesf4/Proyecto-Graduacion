using HFPMapp.ViewModels.Projects.Reports;

namespace HFPMapp.Views.Projects.Reports;

public partial class ListView : ContentPage
{
	public ListView(ListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}