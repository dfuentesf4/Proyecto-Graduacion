using HFPMapp.ViewModels.Login;
namespace HFPMapp.Views.Login;

public partial class LoginView : ContentPage
{

    public LoginView(LoginViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
	}

}