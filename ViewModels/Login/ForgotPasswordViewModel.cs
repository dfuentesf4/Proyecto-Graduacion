using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Login
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        public ForgotPasswordViewModel(UserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
            SendRecoveryLinkCommand = new Command(async () => await SendRecoveryToken());
            ResetPasswordCommand = new Command(async () => await ResetPassword());
        }

		public ICommand SendRecoveryLinkCommand { get; set; }
        public ICommand ResetPasswordCommand { get; set; }
        private UserApiClient _userApiClient;


        private string _user;
		public string User
		{
			get
			{
				return _user;
			}
			set
			{
				_user = value;
				OnPropertyChanged(nameof(User));
			}
		}

		private async Task SendRecoveryToken()
		{
			if (String.IsNullOrEmpty(User))
			{
				ThrowMessage.ShowErrorMessage("Ingrese un Usurio");
				return;
			}
			bool res = await _userApiClient.SendRecoveryToken(User);

			if(res) ThrowMessage.ShowSuccessMessage("Token enviado con exito al correo electronico");
            else ThrowMessage.ShowErrorMessage("Error al enviar el token al correo electronico");
        }

		private async Task ResetPassword()
		{
            if (String.IsNullOrEmpty(User2))
            {
                ThrowMessage.ShowErrorMessage("Ingrese un Usurio");
                return;
            }
            if (IsErrorPasswordVisible)
            {
                ThrowMessage.ShowErrorMessage("Las contraseñas no coinciden");
                return;
            }
			if(String.IsNullOrEmpty(Token))
            {
                ThrowMessage.ShowErrorMessage("Ingrese el token y la nueva contraseña");
                return;
            }

            bool res = await _userApiClient.ResetPasswordAsync(User2, Contraseña, Token);

			if (res)
			{
				ThrowMessage.ShowSuccessMessage("Contraseña Restablecida Correctamente");
			}
        }

		private string _user2;
		public string User2
		{
			get
			{
				return _user2;
			}
			set
			{
				_user2 = value;
				OnPropertyChanged(nameof(User2));
			}
		}

		private string _contraseña;
		public string Contraseña
		{
			get
			{
				return _contraseña;
			}
			set
			{
				_contraseña = value;
				OnPropertyChanged(nameof(Contraseña));
			}
		}

		private string _contraseña2;
		public string Contraseña2
		{
			get
			{
				return _contraseña2;
			}
			set
			{
				_contraseña2 = value;
				OnPropertyChanged(nameof(Contraseña2));
                if (Contraseña == Contraseña2)
                {
                    IsErrorPasswordVisible = false;
                }
                else
                {
                    IsErrorPasswordVisible = true;
                }
            }
		}

		private string _token;
		public string Token
		{
			get
			{
				return _token;
			}
			set
			{
				_token = value;
				OnPropertyChanged(nameof(Token));
			}
		}

		private bool _isErrorPasswordVisible;
		public bool IsErrorPasswordVisible
		{
			get
			{
				return _isErrorPasswordVisible;
			}
			set
			{
				_isErrorPasswordVisible = value;
				OnPropertyChanged(nameof(IsErrorPasswordVisible));
			}
		}
	}
}
