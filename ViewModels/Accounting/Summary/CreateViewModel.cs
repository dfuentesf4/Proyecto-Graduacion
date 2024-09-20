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

namespace HFPMapp.ViewModels.Accounting.Summary
{
    public class CreateViewModel : BaseViewModel
    {
        public CreateViewModel(SummaryApiClient summaryApiClient)
        {
            SummaryApiClient = summaryApiClient;
            CreateCommand = new Command(async () => await CreateSummary());
            _summary = new();
            Months = Enum.GetValues(typeof(MonthsEnum)).Cast<MonthsEnum>().ToList();
        }

        public SummaryApiClient SummaryApiClient { get; set; }
        public ICommand CreateCommand { get; }

        private HFPMapp.Models.Summary _summary;
        public HFPMapp.Models.Summary Summary
        {
            get => _summary;
            set
            {
                _summary = value;
                OnPropertyChanged(nameof(Summary));
            }
        }

        private MonthsEnum _selectedMonth;
        public MonthsEnum SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                _selectedMonth = value;
                Summary.Month = (int)value;
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
            var context = new ValidationContext(Summary, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(Summary, context, results, true);

            if (!isValid)
            {
                ErrorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            }

            return isValid;
        }

        public void ClearFields()
        {
            Summary = new();
        }

        private async Task CreateSummary()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isCreated = await SummaryApiClient.CreateSummaryAsync(Summary);

            if (isCreated)
            {
                ThrowMessage.ShowSuccessMessage("Resumen Creado con éxito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error creando el Resumen");
            }

            IsBusy = false;
        }
    }
}
