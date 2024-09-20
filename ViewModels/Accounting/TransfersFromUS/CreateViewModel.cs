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

namespace HFPMapp.ViewModels.Accounting.TransfersFromUS
{
    public class CreateViewModel : BaseViewModel
    {
        public CreateViewModel(TransfersFromUApiClient transfersFromUApiClient)
        {
            TransfersFromUApiClient = transfersFromUApiClient;
            CreateCommand = new Command(async () => await CreateTransfer());
            TransfersFromU = new TransfersFromU();
        }

        public TransfersFromUApiClient TransfersFromUApiClient { get; set; }
        public ICommand CreateCommand { get; }

        private TransfersFromU _transfersFromU;
        public TransfersFromU TransfersFromU
        {
            get => _transfersFromU;
            set
            {
                _transfersFromU = value;
                OnPropertyChanged(nameof(TransfersFromU));
            }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public bool ValidateModel()
        {
            var context = new ValidationContext(TransfersFromU, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(TransfersFromU, context, results, true);

            if (!isValid)
            {
                ErrorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            }

            return isValid;
        }

        public void ClearFields()
        {
            TransfersFromU = new TransfersFromU();
        }

        private async Task CreateTransfer()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isCreated = await TransfersFromUApiClient.CreateTransfersFromUAsync(TransfersFromU);

            if (isCreated)
            {
                ThrowMessage.ShowSuccessMessage("Transferencia creada con éxito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error creando la transferencia");
            }

            IsBusy = false;
        }
    }
}
