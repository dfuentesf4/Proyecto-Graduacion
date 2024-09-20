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

namespace HFPMapp.ViewModels.Accounting.ExpensesDetails
{
    public class CreateViewModel : BaseViewModel
    {
        public CreateViewModel(ExpensesDetailApiClient expensesDetailApiClient)
        {
            ExpensesDetailApiClient = expensesDetailApiClient;
            CreateCommand = new Command(async () => await CreateExpensesDetail());
            _expensesDetail = new();
            Months = Enum.GetValues(typeof(MonthsEnum)).Cast<MonthsEnum>().ToList();
        }

        public ExpensesDetailApiClient ExpensesDetailApiClient { get; set; }
        public ICommand CreateCommand { get; }

        private ExpensesDetail _expensesDetail;
        public ExpensesDetail ExpensesDetail
        {
            get => _expensesDetail;
            set
            {
                _expensesDetail = value;
                OnPropertyChanged(nameof(ExpensesDetail));
            }
        }

        private MonthsEnum _selectedMonth;
        public MonthsEnum SelectedMonth
        {
            get
            {
                return _selectedMonth;
            }
            set
            {
                _selectedMonth = value;
                ExpensesDetail.Month = (int)value;
                OnPropertyChanged(nameof(SelectedMonth));
            }
        }

        public List<MonthsEnum> Months { get; }

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
            var context = new ValidationContext(ExpensesDetail, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(ExpensesDetail, context, results, true);

            if (!isValid)
            {
                ErrorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            }

            return isValid;
        }

        public void ClearFields()
        {
            ExpensesDetail = new();
        }

        private async Task CreateExpensesDetail()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isCreated = await ExpensesDetailApiClient.CreateExpensesDetailAsync(ExpensesDetail);

            if (isCreated)
            {
                ThrowMessage.ShowSuccessMessage("Detalle de Gasto Creado con éxito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error creando el Detalle de Gasto");
            }

            IsBusy = false;
        }
    }
}
