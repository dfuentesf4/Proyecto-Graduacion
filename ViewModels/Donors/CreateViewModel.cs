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

namespace HFPMapp.ViewModels.Donors
{
    public class CreateViewModel : BaseViewModel
    {
        public CreateViewModel(DonorApiClient donorApiClient)
        {
            DonorApiClient = donorApiClient;
            CreateDonorCommand = new Command(async () => await CreateDonor());
            _donor = new();
        }

        public DonorApiClient DonorApiClient { get; set; }
        public ICommand CreateDonorCommand { get; }


        private Donor _donor;
        public Donor Donor
        {
            get
            {
                return _donor;
            }
            set
            {
                _donor = value;
                OnPropertyChanged(nameof(Donor));
            }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                _IsBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public bool ValidateModel()
        {
            var context = new ValidationContext(Donor, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(Donor, context, results, true);

            if (!isValid)
            {
                ErrorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            }

            return isValid;
        }

        public void ClearFields()
        {
            Donor = new();
        }

        private async Task CreateDonor()
        {
            IsBusy = true;

            bool isModelValid = ValidateModel();

            if (!isModelValid)
            {
                IsBusy = false;
                return;
            }

            bool isCreated = await DonorApiClient.CreateDonorAsync(Donor);

            if (isCreated)
            {
                ThrowMessage.ShowSuccessMessage("Donador Creado con exito");
                await Shell.Current.GoToAsync("//ListDonor");
                ClearFields();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error creando el Donador");
            }

            IsBusy = false;
        }
    }
}
