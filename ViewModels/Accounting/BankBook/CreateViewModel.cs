using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Accounting.BankBook
{ 
    public class CreateViewModel : BaseViewModel
    {
        public CreateViewModel(BankBookApiClient bankBookApiClient, BankApiClient bankApiClient)
        {
            BankBookApiClient = bankBookApiClient;
            BankApiClient = bankApiClient;
            CreateCommand = new Command(async () => await CreateBankBook());
            _bankBook = new();
            LoadBanks();
        }

        public BankBookApiClient BankBookApiClient { get; set; }
        public BankApiClient BankApiClient { get; set; }
        public ICommand CreateCommand { get; }

        private HFPMapp.Models.BankBook _bankBook;
        public HFPMapp.Models.BankBook BankBook
        {
            get => _bankBook;
            set
            {
                _bankBook = value;
                OnPropertyChanged(nameof(BankBook));
            }
        }

        private List<HFPMapp.Models.Bank> _banks;
        public List<HFPMapp.Models.Bank> Banks
        {
            get => _banks;
            set
            {
                _banks = value;
                OnPropertyChanged(nameof(Banks));
            }
        }

        private HFPMapp.Models.Bank _selectedBank;
        public HFPMapp.Models.Bank SelectedBank
        {
            get => _selectedBank;
            set
            {
                _selectedBank = value;
                if (value != null)
                {
                    BankBook.BankId = value.Id;
                    BankBook.Bank = value;
                }
                OnPropertyChanged(nameof(SelectedBank));
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

        public async Task LoadBanks()
        {
            Banks = await BankApiClient.GetBanksAsync();
        }

        public bool ValidateModel()
        {
            var context = new ValidationContext(BankBook, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(BankBook, context, results, true);

            if (!isValid)
            {
                ErrorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            }

            return isValid;
        }

        public void ClearFields()
        {
            BankBook = new();
        }

        private async Task CreateBankBook()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isCreated = await BankBookApiClient.CreateBankBookAsync(BankBook);

            if (isCreated)
            {
                ThrowMessage.ShowSuccessMessage("Bank Book creado con éxito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error creando el Bank Book");
            }

            IsBusy = false;
        }
    }
}
