using HFPMapp.ViewModels.Reports;

namespace HFPMapp.Views.Reports;

public partial class MenuView : ContentPage
{
	public MenuView(MenuViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}