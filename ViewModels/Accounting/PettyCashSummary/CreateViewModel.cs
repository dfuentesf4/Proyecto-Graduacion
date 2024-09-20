﻿using HFPMapp.Models;
using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Accounting.PettyCashSummary
{
    public class CreateViewModel : BaseViewModel
    {
        public CreateViewModel(PettyCashSummaryApiClient pettyCashSummaryApiClient)
        {
            PettyCashSummaryApiClient = pettyCashSummaryApiClient;
            CreateCommand = new Command(async () => await CreatePettyCashSummary());
            _pettyCashSummary = new HFPMapp.Models.PettyCashSummary();
            Months = Enum.GetValues(typeof(MonthsEnum)).Cast<MonthsEnum>().ToList();
        }

        public PettyCashSummaryApiClient PettyCashSummaryApiClient { get; set; }
        public ICommand CreateCommand { get; }

        private HFPMapp.Models.PettyCashSummary _pettyCashSummary;
        public HFPMapp.Models.PettyCashSummary PettyCashSummary
        {
            get => _pettyCashSummary;
            set
            {
                _pettyCashSummary = value;
                OnPropertyChanged(nameof(PettyCashSummary));
            }
        }

        private MonthsEnum _selectedMonth;
        public MonthsEnum SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                _selectedMonth = value;
                PettyCashSummary.Month = (int)value;
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
            var context = new ValidationContext(PettyCashSummary, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(PettyCashSummary, context, results, true);

            if (!isValid)
            {
                ErrorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            }

            return isValid;
        }

        public void ClearFields()
        {
            PettyCashSummary = new HFPMapp.Models.PettyCashSummary();
        }

        private async Task CreatePettyCashSummary()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isCreated = await PettyCashSummaryApiClient.CreatePettyCashSummaryAsync(PettyCashSummary);

            if (isCreated)
            {
                ThrowMessage.ShowSuccessMessage("Resumen de Caja Chica Creado con éxito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error creando el Resumen de Caja Chica");
            }

            IsBusy = false;
        }
    }
}
