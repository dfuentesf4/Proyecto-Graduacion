using HFPMapp.ViewModels.Accounting;

namespace HFPMapp.Views.Accounting;

public partial class MenuView : ContentPage
{
	public MenuView(MenuViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}