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
    [QueryProperty(nameof(Summary), "Summary")]
    public class EditViewModel : BaseViewModel
    {
        public EditViewModel(SummaryApiClient summaryApiClient)
        {
            SummaryApiClient = summaryApiClient;
            EditCommand = new Command(async () => await EditSummary());
            _summary = new();
            Months = Enum.GetValues(typeof(MonthsEnum)).Cast<MonthsEnum>().ToList();

            // Establecer el mes seleccionado basado en el valor del modelo Summary
            if (Summary != null && Summary.Month.HasValue)
            {
                SelectedMonth = (MonthsEnum)Summary.Month.Value;
            }
        }

        public SummaryApiClient SummaryApiClient { get; set; }
        public ICommand EditCommand { get; }

        private HFPMapp.Models.Summary _summary;
        public HFPMapp.Models.Summary Summary
        {
            get => _summary;
            set
            {
                _summary = value;
                if (Summary != null && Summary.Month.HasValue)
                {
                    SelectedMonth = (MonthsEnum)Summary.Month.Value;
                }
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

        private async Task EditSummary()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isEdited = await SummaryApiClient.UpdateSummaryAsync(Summary);

            if (isEdited)
            {
                ThrowMessage.ShowSuccessMessage("Resumen editado con éxito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error editando el Resumen");
            }

            IsBusy = false;
        }
    }
}
