using HFPMapp.ViewModels.Accounting.PettyCashSummary;

namespace HFPMapp.Views.Accounting.PettyCashSummary;

public partial class CreateView : ContentPage
{
	public CreateView(CreateViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}