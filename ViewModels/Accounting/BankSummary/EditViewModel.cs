using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Accounting.BankSummary
{
    [QueryProperty(nameof(BankSummary), "BankSummary")]
    public class EditViewModel : BaseViewModel
    {
        public EditViewModel(BankSummaryApiClient bankSummaryApiClient, BankApiClient bankApiClient)
        {
            BankSummaryApiClient = bankSummaryApiClient;
            BankApiClient = bankApiClient;
            EditCommand = new Command(async () => await EditBankSummary());
            _bankSummary = new HFPMapp.Models.BankSummary();
            LoadBanks();
            Months = Enum.GetValues(typeof(HFPMapp.Models.MonthsEnum)).Cast<HFPMapp.Models.MonthsEnum>().ToList();
        }

        public BankSummaryApiClient BankSummaryApiClient { get; set; }
        public BankApiClient BankApiClient { get; set; }
        public ICommand EditCommand { get; }

        private HFPMapp.Models.BankSummary _bankSummary;
        public HFPMapp.Models.BankSummary BankSummary
        {
            get => _bankSummary;
            set
            {
                _bankSummary = value;
                if (BankSummary != null && BankSummary.Month.HasValue)
                {
                    SelectedMonth = (HFPMapp.Models.MonthsEnum)BankSummary.Month.Value;
                }
                if (BankSummary != null && BankSummary.BankId.HasValue)
                {
                    SelectedBank = Banks.FirstOrDefault(b => b.Id == BankSummary.BankId.Value);
                }
                OnPropertyChanged(nameof(BankSummary));
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
                if (BankSummary != null)
                {
                    BankSummary.BankId = value?.Id;
                    BankSummary.Bank = value;
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
                BankSummary.Month = (int)value;
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

        public async Task LoadBanks()
        {
            Banks = await BankApiClient.GetBanksAsync();

            if (BankSummary != null && BankSummary.BankId.HasValue)
            {
                SelectedBank = Banks.FirstOrDefault(b => b.Id == BankSummary.BankId.Value);
            }
        }

        public bool ValidateModel()
        {
            var context = new ValidationContext(BankSummary, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(BankSummary, context, results, true);

            if (!isValid)
            {
                ErrorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            }

            return isValid;
        }

        public void ClearFields()
        {
            BankSummary = new HFPMapp.Models.BankSummary();
        }

        private async Task EditBankSummary()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isEdited = await BankSummaryApiClient.UpdateBankSummaryAsync(BankSummary);

            if (isEdited)
            {
                ThrowMessage.ShowSuccessMessage("Resumen Bancario Editado con éxito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error editando el Resumen Bancario");
            }

            IsBusy = false;
        }
    }
}
