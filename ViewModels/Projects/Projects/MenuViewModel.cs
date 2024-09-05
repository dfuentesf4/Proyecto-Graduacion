using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Projects
{
    public class MenuViewModel : BaseViewModel
    {
        public MenuViewModel()
        {
            
        }

        public ICommand ProjectsCommand => new Command(async () => await Shell.Current.GoToAsync("ListProject"));
        public ICommand BeneficiariesCommand => new Command(async () => await Shell.Current.GoToAsync("ListBeneficiary"));
        public ICommand VolunteersCommand => new Command(async () => await Shell.Current.GoToAsync("ListVolunteer"));
        public ICommand ReportsCommand => new Command(async () => await Shell.Current.GoToAsync("ListReport"));
        public ICommand ActivitiesCommand => new Command(async () => await Shell.Current.GoToAsync("ListActivity"));

    }
}
