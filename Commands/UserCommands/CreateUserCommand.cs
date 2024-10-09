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

            int isCreated = await _viewModel.UserApiClient.CreateUserAsync(_viewModel.user);

            if (isCreated > 0)
            {
                Privilege privilege = new Privilege() 
                { 
                    UserId = isCreated,
                    ProjectManager = false,
                    DonorManager = false,
                    AccountingManager = false,
                    UsersManager = false

                };
                if (_viewModel.user.JobPosition == "Gerente General")
                {
                    privilege.ProjectManager = true;
                    privilege.DonorManager = true;
                    privilege.AccountingManager = true;
                    privilege.UsersManager = true;
                }
                else if(_viewModel.user.JobPosition == "Lider de Proyectos")
                {
                    privilege.ProjectManager = true;
                }
                else if (_viewModel.user.JobPosition == "Relaciones Publicas")
                {
                    privilege.DonorManager = true;
                }
                else if (_viewModel.user.JobPosition == "Contador")
                {
                    privilege.AccountingManager = true;
                }

                bool isPrivilegeCreated = await _viewModel.PrivilegeApiClient.CreatePrivilegeAsync(privilege);

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
