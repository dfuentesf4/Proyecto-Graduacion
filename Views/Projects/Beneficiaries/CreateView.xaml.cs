using HFPMapp.ViewModels.Projects.Beneficiaries;

namespace HFPMapp.Views.Projects.Beneficiaries;

public partial class CreateView : ContentPage
{
	public CreateView(CreateViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}