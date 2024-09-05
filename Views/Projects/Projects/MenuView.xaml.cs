using HFPMapp.ViewModels.Projects;

namespace HFPMapp.Views.Projects;

public partial class MenuView : ContentPage
{
	public MenuView(MenuViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}