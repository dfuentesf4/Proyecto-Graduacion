using HFPMapp.ViewModels.Login;

namespace HFPMapp.Views.Login;

public partial class ForgotPasswordView : ContentPage
{
	public ForgotPasswordView(ForgotPasswordViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}