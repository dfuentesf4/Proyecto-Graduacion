using HFPMapp.Commands.LoginCommands;
using HFPMapp.Services;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Login
{
    public class LoginViewModel : BaseViewModel
    {
        public UserApiClient _userApiClient;
        public UserSessionService _userSessionService;
        public ICommand ForgotPasswordCommand => new Command(async () => await Shell.Current.GoToAsync("ForgotPassword"));

        public LoginViewModel(UserApiClient userApiClient, UserSessionService userSessionService)
        {
            _userApiClient = userApiClient;
            _userSessionService = userSessionService;
            LoginCommand = new LoginCommand(this);
    }

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private bool _isErrorVisible;
        public bool IsErrorVisible
        {
            get => _isErrorVisible;
            set => SetProperty(ref _isErrorVisible, value);
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                _IsBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public ICommand LoginCommand { get; }


        
    }
}
