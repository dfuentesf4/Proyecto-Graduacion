using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Accounting.FolderBank
{
    public class CreateViewModel : BaseViewModel
    {
        public CreateViewModel(FolderBankApiClient folderBankApiClient, BankApiClient bankApiClient)
        {
            FolderBankApiClient = folderBankApiClient;
            BankApiClient = bankApiClient;
            CreateCommand = new Command(async () => await CreateFolderBank());
            _folderBank = new HFPMapp.Models.FolderBank();
            LoadBanks();
            Months = Enum.GetValues(typeof(HFPMapp.Models.MonthsEnum)).Cast<HFPMapp.Models.MonthsEnum>().ToList();
        }

        public FolderBankApiClient FolderBankApiClient { get; set; }
        public BankApiClient BankApiClient { get; set; }
        public ICommand CreateCommand { get; }

        private HFPMapp.Models.FolderBank _folderBank;
        public HFPMapp.Models.FolderBank FolderBank
        {
            get => _folderBank;
            set
            {
                _folderBank = value;
                OnPropertyChanged(nameof(FolderBank));
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
                if (FolderBank != null)
                {
                    FolderBank.BankId = value?.Id;
                    FolderBank.Bank = value;
                }
                OnPropertyChanged(nameof(SelectedBank));
            }
        }

        private HFPMapp.Models.MonthsEnum _selectedMonth;
        public HFPMapp.Models.MonthsEnum SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                _selectedMonth = value;
                FolderBank.Month = (int)value;
                OnPropertyChanged(nameof(SelectedMonth));
            }
        }

        public List<HFPMapp.Models.MonthsEnum> Months { get; }

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
            var context = new ValidationContext(FolderBank, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(FolderBank, context, results, true);

            if (!isValid)
            {
                ErrorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            }

            return isValid;
        }

        public async Task LoadBanks()
        {
            Banks = await BankApiClient.GetBanksAsync();
        }

        public async void ClearFields()
        {
            FolderBank = new HFPMapp.Models.FolderBank();
            SelectedBank = null;
            SelectedMonth = HFPMapp.Models.MonthsEnum.Enero; // Default to January
        }

        private async Task CreateFolderBank()
        {
            IsBusy = true;

            if (!ValidateModel())
            {
                IsBusy = false;
                return;
            }

            bool isCreated = await FolderBankApiClient.CreateFolderBankAsync(FolderBank);

            if (isCreated)
            {
                ThrowMessage.ShowSuccessMessage("FolderBank creado con éxito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error al crear el FolderBank");
            }

            IsBusy = false;
        }
    }
}
