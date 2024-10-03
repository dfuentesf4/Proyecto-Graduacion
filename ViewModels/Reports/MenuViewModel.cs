using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Reports
{
    public class MenuViewModel : BaseViewModel
    {
        public ICommand AccountingCommand => new Command(async () => await Shell.Current.GoToAsync("AccountingReports"));
        public ICommand ProjectsCommand => new Command(async () => await Shell.Current.GoToAsync("ProjectsReports"));
    }
}
