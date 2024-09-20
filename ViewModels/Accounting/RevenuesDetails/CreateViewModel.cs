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
    public class CreateViewModel : BaseViewModel
    {
        public CreateViewModel(RevenuesDetailApiClient revenuesDetailApiClient)
        {
            RevenuesDetailApiClient = revenuesDetailApiClient;
            CreateCommand = new Command(async () => await CreateRevenuesDetail());
            _revenuesDetail = new();
            Months = Enum.GetValues(typeof(MonthsEnum)).Cast<MonthsEnum>().ToList();
        }

        public RevenuesDetailApiClient RevenuesDetailApiClient { get; set; }
        public ICommand CreateCommand { get; }

        private RevenuesDetail _revenuesDetail;
        public RevenuesDetail RevenuesDetail
        {
            get => _revenuesDetail;
            set
            {
                _revenuesDetail = value;
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

        private async Task CreateRevenuesDetail()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isCreated = await RevenuesDetailApiClient.CreateRevenuesDetailAsync(RevenuesDetail);

            if (isCreated)
            {
                ThrowMessage.ShowSuccessMessage("Detalle de Ingreso Creado con éxito");
                await Shell.Current.Navigation.PopAsync();
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error creando el Detalle de Ingreso");
            }

            IsBusy = false;
        }
    }
}
