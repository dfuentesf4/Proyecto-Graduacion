using HFPMapp.ViewModels.Accounting.BankBook;

namespace HFPMapp.Views.Accounting.BankBook;

public partial class CreateView : ContentPage
{
	public CreateView(CreateViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}