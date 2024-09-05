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

    [QueryProperty(nameof(Donor), "Donor")]
    public class EditViewModel : BaseViewModel
    {
        public EditViewModel(DonorApiClient donorApiClient)
        {
            DonorApiClient = donorApiClient;
        }

        public DonorApiClient DonorApiClient { get; set; }
        public ICommand EditCommand => new Command(EditDonor);


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

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
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

        public async void EditDonor()
        {
            IsBusy = true;
            bool isModelValid = ValidateModel();

            if (!isModelValid) return;

            bool isEdited = await DonorApiClient.UpdateDonorAsync(Donor);

            if (isEdited)
            {
                ThrowMessage.ShowSuccessMessage("Donador Editado con exito");
                await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                ThrowMessage.ShowErrorMessage("Hubo un error Editando el Donador");
            }
            IsBusy = false;
        }
    }
}
