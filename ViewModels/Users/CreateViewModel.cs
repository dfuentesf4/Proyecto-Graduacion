using HFPMapp.Commands.UserCommands;
using HFPMapp.Models;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Users
{
    public class CreateViewModel : BaseViewModel
    {
        public CreateViewModel(UserApiClient userApiClient)
        {
            UserApiClient = userApiClient;
            CreateUserCommand = new CreateUserCommand(this);
            user = new();
        }

        public UserApiClient UserApiClient { get; set; }

        private string? _name;
        private string? _lastname;
        private string _email = string.Empty;
        private string? _phoneNumber;
        private string? _jobPosition;
        private DateTime _birthDate = DateTime.Today;
        private string? _gender;
        private string _userName = string.Empty;
        private string _password = string.Empty;
        private string _password2 = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isErrorPassworVisible = false;
        private bool _isActive = true;
        public User user;
        public ICommand CreateUserCommand { get; }

        public string? Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                user.FirstName = value;
            }
        }

        public string? LastName
        {
            get => _lastname;
            set
            {
                SetProperty(ref _lastname, value);
                user.LastName = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                user.Email = value;
            }
        }

        public string? PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                SetProperty(ref _phoneNumber, value);
                user.PhoneNumber = value;
            }
        }

        public string? JobPosition
        {
            get => _jobPosition;
            set
            {
                SetProperty(ref _jobPosition, value);
                user.JobPosition = value;
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                SetProperty(ref _birthDate, value);
                user.BirthDate = DateOnly.FromDateTime(value);
            }
        }

        public string? Gender
        {
            get => _gender;
            set
            {
                SetProperty(ref _gender, value);
                user.Gender = value;
            }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                SetProperty(ref _userName, value);
                user.UserName = value;
            }
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string Password2
        {
            get => _password2;
            set
            {
                SetProperty(ref _password2, value);
                if (Password == Password2)
                {
                    user.Password = value;
                    IsErrorPasswordVisible = false;
                }
                else
                {
                    user.Password = string.Empty;
                    IsErrorPasswordVisible = true;
                }
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public bool IsErrorPasswordVisible
        {
            get => _isErrorPassworVisible;
            set => SetProperty(ref _isErrorPassworVisible, value);
        }

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
            var context = new ValidationContext(user, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(user, context, results, true);

            if (!isValid)
            {
                ErrorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            }

            return isValid;
        }

        public void ClearFields()
        {
            Name = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            JobPosition = string.Empty;
            BirthDate = DateTime.Today;
            UserName = string.Empty;
            Password = string.Empty;
            Password2 = string.Empty;
        }
    }
}
