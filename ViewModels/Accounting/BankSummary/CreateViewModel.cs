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
    public class CreateViewModel : BaseViewModel
    {
        public CreateViewModel(BankSummaryApiClient bankSummaryApiClient, BankApiClient bankApiClient)
        {
            BankSummaryApiClient = bankSummaryApiClient;
            BankApiClient = bankApiClient;
            CreateCommand = new Command(async () => await CreateBankSummary());
            _bankSummary = new HFPMapp.Models.BankSummary();
            Months = Enum.GetValues(typeof(HFPMapp.Models.MonthsEnum)).Cast<HFPMapp.Models.MonthsEnum>().ToList();
            LoadBanks();
        }

        public BankSummaryApiClient BankSummaryApiClient { get; set; }
        public BankApiClient BankApiClient { get; set; }
        public ICommand CreateCommand { get; }

        private HFPMapp.Models.BankSummary _bankSummary;
        public HFPMapp.Models.BankSummary BankSummary
        {
            get => _bankSummary;
            set
            {
                _bankSummary = value;
                OnPropertyChanged(nameof(BankSummary));
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

        private HFPMapp.Models.Bank _selectedBank;
        public HFPMapp.Models.Bank SelectedBank
        {
            get => _selectedBank;
            set
            {
                _selectedBank = value;
                BankSummary.BankId = value?.Id;
                OnPropertyChanged(nameof(SelectedBank));
            }
        }

        public List<HFPMapp.Models.MonthsEnum> Months { get; }
        private List<HFPMapp.Models.Bank> _Banks;
        public List<HFPMapp.Models.Bank> Banks
        {
            get
            {
                return _Banks;
            }
            set
            {
                _Banks = value;
                OnPropertyChanged(nameof(Banks));
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
            BankSummary = new();
        }

        public async Task LoadBanks()
        {
            Banks = await BankApiClient.GetBanksAsync();
        }

        private async Task CreateBankSummary()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isCreated = await BankSummaryApiClient.CreateBankSummaryAsync(BankSummary);

            if (isCreated)
            {
                ThrowMessage.ShowSuccessMessage("Resumen Bancario creado con éxito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error creando el Resumen Bancario");
            }

            IsBusy = false;
        }
    }
}
