using HFPMapp.ViewModels.Projects.Volunteers;

namespace HFPMapp.Views.Projects.Volunteers;

public partial class ListView : ContentPage
{
	public ListView(ListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}