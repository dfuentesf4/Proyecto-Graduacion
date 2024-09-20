using HFPMapp.Services.Alerts;
using HFPMapp.Services.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Accounting.FolderBank
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel(FolderBankApiClient folderBankApiClient)
        {
            FolderBankApiClient = folderBankApiClient;
            LoadFolderBanks();
        }

        public FolderBankApiClient FolderBankApiClient { get; set; }
        public ICommand DeleteCommand => new Command<HFPMapp.Models.FolderBank>(DeleteFolderBank);
        public ICommand EditCommand => new Command<HFPMapp.Models.FolderBank>(EditFolderBank);
        public ICommand RefreshCommand => new Command(async () => await LoadFolderBanks());
        public ICommand SearchCommand => new Command(async () => await FilterFolderBankAsync());
        public ICommand CreateCommand => new Command(async () => await Shell.Current.GoToAsync("CreateFolderBank"));

        private List<HFPMapp.Models.FolderBank> _allFolderBanks;
        public List<HFPMapp.Models.FolderBank> AllFolderBanks
        {
            get => _allFolderBanks;
            set
            {
                _allFolderBanks = value;
                OnPropertyChanged(nameof(AllFolderBanks));
            }
        }

        private List<HFPMapp.Models.FolderBank> _folderBanks;
        public List<HFPMapp.Models.FolderBank> FolderBanks
        {
            get => _folderBanks;
            set
            {
                _folderBanks = value;
                OnPropertyChanged(nameof(FolderBanks));
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private bool _seeInactives;
        public bool SeeInactives
        {
            get => _seeInactives;
            set
            {
                _seeInactives = value;
                OnPropertyChanged(nameof(SeeInactives));
            }
        }

        public async Task LoadFolderBanks()
        {
            IsRefreshing = true;
            AllFolderBanks = await FolderBankApiClient.GetFolderBanksAsync();
            FilterFolderBanks();
            IsRefreshing = false;
        }

        public async void DeleteFolderBank(HFPMapp.Models.FolderBank folderBank)
        {
            folderBank.IsActive = false;
            bool response = await FolderBankApiClient.UpdateFolderBankAsync(folderBank);
            if (response)
                ThrowMessage.ShowSuccessMessage("Folder Bank eliminado correctamente");
            else
                ThrowMessage.ShowErrorMessage("Error al eliminar Folder Bank");
            await LoadFolderBanks();
        }

        public async void EditFolderBank(HFPMapp.Models.FolderBank folderBank)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "FolderBank" , folderBank }
            };

            await Shell.Current.GoToAsync("EditFolderBank", parameters);
        }

        public async Task FilterFolderBankAsync()
        {
            try
            {
                FilterFolderBanks();
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to filter folder banks: " + ex.Message);
            }
        }

        private void FilterFolderBanks()
        {
            var filteredFolderBanks = AllFolderBanks;

            // Filtrar detalles de folder banks por el texto de búsqueda en Año, Mes, Carpeta, etc.
            if (!string.IsNullOrEmpty(SearchText))
            {
                filteredFolderBanks = filteredFolderBanks.Where(fb =>
                    (fb.Year.HasValue && fb.Year.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (fb.Month.HasValue && fb.Month.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(fb.Folder) && fb.Folder.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (fb.Bank != null && fb.Bank.Name != null && fb.Bank.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            if (SeeInactives)
            {
                FolderBanks = filteredFolderBanks.Where(fb => fb.IsActive == false).ToList();
            }
            else
            {
                FolderBanks = filteredFolderBanks.Where(fb => fb.IsActive == true).ToList();
            }
        }
    }
}
