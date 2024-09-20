using HFPMapp.ViewModels.Accounting.Summary;

namespace HFPMapp.Views.Accounting.Summary;

public partial class CreateView : ContentPage
{
	public CreateView(CreateViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}