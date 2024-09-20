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

namespace HFPMapp.ViewModels.Accounting.RevenuesDetails
{
    [QueryProperty(nameof(RevenuesDetail), "RevenuesDetail")]
    public class EditViewModel : BaseViewModel
    {
        public EditViewModel(RevenuesDetailApiClient revenuesDetailApiClient)
        {
            RevenuesDetailApiClient = revenuesDetailApiClient;
            EditCommand = new Command(async () => await EditRevenuesDetail());
            _revenuesDetail = new();
            Months = Enum.GetValues(typeof(MonthsEnum)).Cast<MonthsEnum>().ToList();

            if (RevenuesDetail != null && RevenuesDetail.Month.HasValue)
            {
                SelectedMonth = (MonthsEnum)RevenuesDetail.Month.Value;
            }
        }

        public RevenuesDetailApiClient RevenuesDetailApiClient { get; set; }
        public ICommand EditCommand { get; }

        private RevenuesDetail _revenuesDetail;
        public RevenuesDetail RevenuesDetail
        {
            get => _revenuesDetail;
            set
            {
                _revenuesDetail = value;
                if (RevenuesDetail != null && RevenuesDetail.Month.HasValue)
                {
                    SelectedMonth = (MonthsEnum)RevenuesDetail.Month.Value;
                }
                OnPropertyChanged(nameof(RevenuesDetail));
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
                RevenuesDetail.Month = (int)value;

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
            var context = new ValidationContext(RevenuesDetail, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(RevenuesDetail, context, results, true);

            if (!isValid)
            {
                ErrorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            }

            return isValid;
        }

        public void ClearFields()
        {
            RevenuesDetail = new();
        }

        private async Task EditRevenuesDetail()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isEdited = await RevenuesDetailApiClient.UpdateRevenuesDetailAsync(RevenuesDetail);

            if (isEdited)
            {
                ThrowMessage.ShowSuccessMessage("Detalle de Ingreso editado con éxito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error editando el Detalle de Ingreso");
            }

            IsBusy = false;
        }
    }
}
