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

namespace HFPMapp.ViewModels.Accounting.TransfersSummary
{
    [QueryProperty(nameof(TransferSummary), "TransferSummary")]
    public class EditViewModel : BaseViewModel
    {
        public EditViewModel(TransfersSummaryApiClient transferSummaryApiClient)
        {
            TransferSummaryApiClient = transferSummaryApiClient;
            EditCommand = new Command(async () => await EditTransferSummary());
            _transferSummary = new();
        }

        public TransfersSummaryApiClient TransferSummaryApiClient { get; set; }
        public ICommand EditCommand { get; }

        private TransferSummary _transferSummary;
        public TransferSummary TransferSummary
        {
            get => _transferSummary;
            set
            {
                _transferSummary = value;
                OnPropertyChanged(nameof(TransferSummary));
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
            var context = new ValidationContext(TransferSummary, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(TransferSummary, context, results, true);

            if (!isValid)
            {
                ErrorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            }

            return isValid;
        }

        public void ClearFields()
        {
            TransferSummary = new();
        }

        private async Task EditTransferSummary()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isEdited = await TransferSummaryApiClient.UpdateTransferSummaryAsync(TransferSummary);

            if (isEdited)
            {
                ThrowMessage.ShowSuccessMessage("Resumen de Transferencia editado con éxito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error editando el Resumen de Transferencia");
            }

            IsBusy = false;
        }
    }
}
