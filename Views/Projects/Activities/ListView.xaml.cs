using HFPMapp.ViewModels.Projects.Activities;

namespace HFPMapp.Views.Projects.Activities;

public partial class ListView : ContentPage
{
	public ListView(ListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}