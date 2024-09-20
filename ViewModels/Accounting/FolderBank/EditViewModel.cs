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
    [QueryProperty(nameof(FolderBank), "FolderBank")]
    public class EditViewModel : BaseViewModel
    {
        public EditViewModel(FolderBankApiClient folderBankApiClient, BankApiClient bankApiClient)
        {
            FolderBankApiClient = folderBankApiClient;
            BankApiClient = bankApiClient;
            EditCommand = new Command(async () => await EditFolderBank());
            LoadBanks();
            Months = Enum.GetValues(typeof(HFPMapp.Models.MonthsEnum)).Cast<HFPMapp.Models.MonthsEnum>().ToList();
        }

        public FolderBankApiClient FolderBankApiClient { get; set; }
        public BankApiClient BankApiClient { get; set; }
        public ICommand EditCommand { get; }

        private HFPMapp.Models.FolderBank _folderBank;
        public HFPMapp.Models.FolderBank FolderBank
        {
            get => _folderBank;
            set
            {
                _folderBank = value;
                if (FolderBank != null && FolderBank.Month.HasValue)
                {
                    SelectedMonth = (HFPMapp.Models.MonthsEnum)FolderBank.Month.Value;
                }
                if (Banks != null && FolderBank.BankId.HasValue)
                {
                    SelectedBank = Banks.FirstOrDefault(b => b.Id == FolderBank.BankId);
                }
                OnPropertyChanged(nameof(FolderBank));
            }
        }

        private HFPMapp.Models.MonthsEnum _selectedMonth;
        public HFPMapp.Models.MonthsEnum SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                _selectedMonth = value;
                if(FolderBank is not null) FolderBank.Month = (int)value;
                OnPropertyChanged(nameof(SelectedMonth));
            }
        }

        public List<HFPMapp.Models.MonthsEnum> Months { get; }

        private HFPMapp.Models.Bank _selectedBank;
        public HFPMapp.Models.Bank SelectedBank
        {
            get => _selectedBank;
            set
            {
                _selectedBank = value;
                FolderBank.BankId = value?.Id;
                OnPropertyChanged(nameof(SelectedBank));
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

        private string _errorMessage;
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
            if (FolderBank != null && FolderBank.BankId.HasValue)
            {
                SelectedBank = Banks.FirstOrDefault(b => b.Id == FolderBank.BankId);
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

        public void ClearFields()
        {
            FolderBank = new();
        }

        private async Task EditFolderBank()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isUpdated = await FolderBankApiClient.UpdateFolderBankAsync(FolderBank);

            if (isUpdated)
            {
                ThrowMessage.ShowSuccessMessage("FolderBank actualizado con éxito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error al actualizar el FolderBank");
            }

            IsBusy = false;
        }
    }
}
