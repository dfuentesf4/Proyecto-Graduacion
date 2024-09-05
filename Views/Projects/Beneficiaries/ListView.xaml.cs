using HFPMapp.ViewModels.Projects.Beneficiaries;

namespace HFPMapp.Views.Projects.Beneficiaries;

public partial class ListView : ContentPage
{
	public ListView(ListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}