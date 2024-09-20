using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFPMapp.ViewModels.Accounting
{
    public class MenuViewModel : BaseViewModel
    {

        public ICommand ExpensesDetailCommand => new Command(async () => await Shell.Current.GoToAsync("ListExpensesDetail"));
        public ICommand RevenuesDetailCommand => new Command(async () => await Shell.Current.GoToAsync("ListRevenuesDetail"));
        public ICommand SummaryCommand => new Command(async () => await Shell.Current.GoToAsync("ListSummary"));
        public ICommand PettyCashSummaryCommand => new Command(async () => await Shell.Current.GoToAsync("ListPettyCashSummary"));
        public ICommand TransfersFromUS => new Command(async () => await Shell.Current.GoToAsync("ListTransfersFromU"));
        public ICommand TransfersSummaryCommand => new Command(async () => await Shell.Current.GoToAsync("ListTransferSummary"));
        public ICommand BankCommand => new Command(async () => await Shell.Current.GoToAsync("ListBank"));
        public ICommand FolderBankCommand => new Command(async () => await Shell.Current.GoToAsync("ListFolderBank"));
        public ICommand BankSummaryCommand => new Command(async () => await Shell.Current.GoToAsync("ListBankSummary"));
        public ICommand BankBookCommand => new Command(async () => await Shell.Current.GoToAsync("ListBankBook"));
    }
}
