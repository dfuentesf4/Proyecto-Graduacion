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
    [QueryProperty(nameof(BankBook), "BankBook")]
    public class EditViewModel : BaseViewModel
    {
        public EditViewModel(BankBookApiClient bankBookApiClient, BankApiClient bankApiClient)
        {
            BankBookApiClient = bankBookApiClient;
            BankApiClient = bankApiClient;
            EditCommand = new Command(async () => await EditBankBook());
            LoadBanks();
        }

        public BankBookApiClient BankBookApiClient { get; set; }
        public BankApiClient BankApiClient { get; set; }
        public ICommand EditCommand { get; }

        private HFPMapp.Models.BankBook _bankBook;
        public HFPMapp.Models.BankBook BankBook
        {
            get => _bankBook;
            set
            {
                _bankBook = value;
                if (Banks != null && BankBook.BankId.HasValue)
                {
                    SelectedBank = Banks.FirstOrDefault(b => b.Id == BankBook.BankId);
                }
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
                if (BankBook != null)
                {
                    BankBook.BankId = value?.Id;
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
            if (BankBook != null && BankBook.BankId.HasValue)
            {
                SelectedBank = Banks.FirstOrDefault(b => b.Id == BankBook.BankId);
            }
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
            BankBook = new HFPMapp.Models.BankBook();
        }

        private async Task EditBankBook()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isEdited = await BankBookApiClient.UpdateBankBookAsync(BankBook);

            if (isEdited)
            {
                ThrowMessage.ShowSuccessMessage("Libro Bancario editado con éxito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error editando el Libro Bancario");
            }

            IsBusy = false;
        }
    }
}
