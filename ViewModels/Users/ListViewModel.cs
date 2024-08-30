using HFPMapp.Models;
using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Users
{
    public class ListViewModel : BaseViewModel
    {

        public ListViewModel(UserApiClient userApiClient)
        {
            UserApiClient = userApiClient;
            LoadUsers();
        }

        public UserApiClient UserApiClient { get; set; }
        public ICommand DeleteCommand => new Command<User>(DeleteUser);
        public ICommand EditCommand => new Command<User>(EditUser);
        public ICommand RefreshCommand => new Command(async () => await LoadUsers());
        public ICommand SearchCommand => new Command(async () => await FilterUserAsync());
        public ICommand CreateCommand => new Command(async () => await Shell.Current.GoToAsync("CreateUser"));

        private List<User> _AllUsers;
        public List<User> AllUsers
        {
            get
            {
                return _AllUsers;
            }
            set
            {
                _AllUsers = value;
                OnPropertyChanged(nameof(AllUsers));
            }
        }

        private List<User> _users;
		public List<User> Users
		{
			get
			{
				return _users;
			}
			set
			{
				_users = value;
				OnPropertyChanged(nameof(Users));
			}
		}

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        private string _SeachText;
        public string SearchText
        {
            get
            {
                return _SeachText;
            }
            set
            {
                _SeachText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private bool _seeInactives;
        public bool SeeInactives
        {
            get
            {
                return _seeInactives;
            }
            set
            {
                _seeInactives = value;
                OnPropertyChanged(nameof(SeeInactives));
            }
        }

        public async Task LoadUsers()
        {
            IsRefreshing = true;
            AllUsers = await UserApiClient.GetUsersAsync();
            if(SeeInactives) Users = AllUsers.Where(u => u.IsActive == false).ToList();
            else Users = AllUsers.Where(u => u.IsActive == true).ToList();
            IsRefreshing = false;
        }

        public async void DeleteUser(User user)
        {
            user.IsActive = false;
            bool response = await UserApiClient.UpdateUserAsync(user);
            if(response) ThrowMessage.ShowSuccessMessage("Usuario eliminado correctamente");
            else ThrowMessage.ShowErrorMessage("Error al eliminar el usuario");
            await LoadUsers();
        }

        public async void EditUser(User user)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "User" , user }
            };

            await Shell.Current.GoToAsync($"EditUser", parameters);
        }

        public async Task FilterUserAsync()
        {
            try
            {
                // Obtener todos los usuarios
                List<User> allUsers = AllUsers;

                // Filtrar usuarios por el texto de búsqueda en FirstName, LastName, Email o PhoneNumber
                var filteredUsers = allUsers.Where(u =>
                    (!string.IsNullOrEmpty(u.FirstName) && u.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(u.LastName) && u.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(u.PhoneNumber) && u.PhoneNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                ).ToList();

                if (SeeInactives) Users = filteredUsers.Where(u => u.IsActive == false).ToList();
                else Users = filteredUsers.Where(u => u.IsActive == true).ToList();

            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to filter users: " + ex.Message);
            }
        }
    }
}
