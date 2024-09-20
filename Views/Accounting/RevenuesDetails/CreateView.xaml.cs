using HFPMapp.ViewModels.Accounting.RevenuesDetails;

namespace HFPMapp.Views.Accounting.RevenuesDetails;

public partial class CreateView : ContentPage
{
	public CreateView(CreateViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}