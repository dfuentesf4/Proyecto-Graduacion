using HFPMapp.ViewModels.Projects.Reports;

namespace HFPMapp.Views.Projects.Reports;

public partial class CreateView : ContentPage
{
	public CreateView(CreateViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}