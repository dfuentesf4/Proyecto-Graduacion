using HFPMapp.Models;
using HFPMapp.Services.Alerts;
using HFPMapp.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Commands.UserCommands
{
    public class CreateUserCommand : CommandBase
    {
        private CreateViewModel _viewModel;

        public CreateUserCommand(CreateViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public async override void Execute(object? parameter)
        {
            _viewModel.IsBusy = true;

            bool isModelValid = _viewModel.ValidateModel();

            if (!isModelValid) return;

            bool isCreated = await _viewModel.UserApiClient.CreateUserAsync(_viewModel.user);

            if (isCreated)
            {
                ThrowMessage.ShowSuccessMessage("Usuario Creado con exito");
                await Shell.Current.GoToAsync("//ListUser");
                _viewModel.ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error creando el usuario");
            }

            _viewModel.IsBusy = false;
        }
    }
}
