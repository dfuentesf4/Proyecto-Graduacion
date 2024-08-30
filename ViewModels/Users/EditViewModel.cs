using HFPMapp.Models;
using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Users
{
    [QueryProperty(nameof(User), "User")]
    public class EditViewModel : BaseViewModel
    {
        public EditViewModel(UserApiClient userApiClient)
        {
            UserApiClient = userApiClient;
        }

        public UserApiClient UserApiClient { get; set; }
        public ICommand EditUserCommand => new Command(EditUser);


        private User _user;
        public User User
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

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
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

        public bool ValidateModel()
        {
            var context = new ValidationContext(User, null, null);
            var results = new List<ValidationResult>();

            // Validar cada propiedad individualmente excepto la de Password
            foreach (var property in typeof(User).GetProperties())
            {
                // Excluir la propiedad Password de la validación
                if (property.Name == nameof(User.Password))
                    continue;

                // Crear un contexto de validación para la propiedad actual
                var propertyContext = new ValidationContext(User)
                {
                    MemberName = property.Name
                };

                // Obtener el valor de la propiedad
                var propertyValue = property.GetValue(User);

                // Validar la propiedad específica
                Validator.TryValidateProperty(propertyValue, propertyContext, results);
            }

            // Asignar los mensajes de error, excluyendo la contraseña que ya no se valida
            if (results.Any())
            {
                ErrorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            }

            return !results.Any();
        }

        public async void EditUser()
        {
            IsBusy = true;
            bool isModelValid = ValidateModel();

            if (!isModelValid) return;

            bool isEdited = await UserApiClient.UpdateUserAsync(User);

            if (isEdited)
            {
                ThrowMessage.ShowSuccessMessage("Usuario Editado con exito");
                await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error Editando el usuario");
            }
            IsBusy = false;
        }
    }
}
