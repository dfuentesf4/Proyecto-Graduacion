using HFPMapp.Services.Alerts;
using HFPMapp.ViewModels.Login;
using HFPMapp.Views.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Commands.LoginCommands
{
    
    public class LoginCommand : CommandBase
    {
        private LoginViewModel _viewModel;

        public LoginCommand(LoginViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public async override void Execute(object? parameter)
        {
            _viewModel.IsBusy = true;
            bool hasAccess =  await _viewModel._userApiClient.LoginAsync(_viewModel.Username, _viewModel.Password, _viewModel);

            if (hasAccess)
            {
                _viewModel.IsErrorVisible = false;
                _viewModel.Username = String.Empty;
                _viewModel.Password = String.Empty;
                await Shell.Current.GoToAsync("//Home");
                Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Invalid username or password");
                _viewModel.ErrorMessage = "Invalid username or password";
                _viewModel.IsErrorVisible = true;
            }
            _viewModel.IsBusy = false;
        }
    }
}
