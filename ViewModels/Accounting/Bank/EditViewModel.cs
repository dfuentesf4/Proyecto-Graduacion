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
    [QueryProperty(nameof(Bank), "Bank")]
    public class EditViewModel : BaseViewModel
    {
        public EditViewModel(BankApiClient bankApiClient)
        {
            BankApiClient = bankApiClient;
            EditCommand = new Command(async () => await EditBank());
        }

        public BankApiClient BankApiClient { get; set; }
        public ICommand EditCommand { get; }

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

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
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

        private async Task EditBank()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isEdited = await BankApiClient.UpdateBankAsync(Bank);

            if (isEdited)
            {
                ThrowMessage.ShowSuccessMessage("Banco editado con éxito");
                await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error al editar el banco");
            }

            IsBusy = false;
        }
    }
}
