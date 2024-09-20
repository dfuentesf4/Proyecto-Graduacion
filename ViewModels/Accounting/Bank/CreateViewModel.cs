using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Accounting.Bank
{
    public class CreateViewModel : BaseViewModel
    {
        public CreateViewModel(BankApiClient bankApiClient)
        {
            BankApiClient = bankApiClient;
            CreateCommand = new Command(async () => await CreateBank());
            Bank = new HFPMapp.Models.Bank();
        }

        public BankApiClient BankApiClient { get; set; }
        public ICommand CreateCommand { get; }

        private HFPMapp.Models.Bank _bank;
        public HFPMapp.Models.Bank Bank
        {
            get => _bank;
            set
            {
                _bank = value;
                OnPropertyChanged(nameof(Bank));
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
            var context = new ValidationContext(Bank, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(Bank, context, results, true);

            if (!isValid)
            {
                ErrorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            }

            return isValid;
        }

        public void ClearFields()
        {
            Bank = new HFPMapp.Models.Bank();
        }

        private async Task CreateBank()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isCreated = await BankApiClient.CreateBankAsync(Bank);

            if (isCreated)
            {
                ThrowMessage.ShowSuccessMessage("Banco creado con éxito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error creando el Banco");
            }

            IsBusy = false;
        }
    }
}
