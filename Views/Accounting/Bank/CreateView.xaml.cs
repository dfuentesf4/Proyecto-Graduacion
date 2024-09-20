using HFPMapp.ViewModels.Accounting.Bank;

namespace HFPMapp.Views.Accounting.Bank;

public partial class CreateView : ContentPage
{
	public CreateView(CreateViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}