using HFPMapp.Services.HTTP;
using HFPMapp.Services.Reports.Chargers;
using HFPMapp.Services.Reports.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.ViewModels.Reports.Accounting
{
    public class AccountingReportsViewModel : BaseViewModel
    {
        public AccountingReportsViewModel(ExpensesDetailApiClient expensesDetailApiClient
            ,RevenuesDetailApiClient revenuesDetailApiClient
            ,TransfersFromUApiClient transfersFromUApiClient)
        {
            _expensesDetailApiClient = expensesDetailApiClient;
            _revenuesDetailApiClient = revenuesDetailApiClient;
            _transfersFromUApiClient = transfersFromUApiClient;
            expensesCharger = new(_expensesDetailApiClient);
            revenuesCharger = new(_revenuesDetailApiClient);
            transfersUsCharger = new(_transfersFromUApiClient);

            ColorModel = new ObservableCollection<Brush>()
            {
                new SolidColorBrush(Color.FromArgb("#08ff00")),
                new SolidColorBrush(Color.FromArgb("#ff0000")),
            };

            LoadExpensesPerYear();
            LoadRevenuesPerYear();
            LoadTransfersUsPerYear();
        }

        private ExpensesDetailApiClient _expensesDetailApiClient;
        private RevenuesDetailApiClient _revenuesDetailApiClient;
        private TransfersFromUApiClient _transfersFromUApiClient;
        private ExpensesCharger expensesCharger;
        private RevenuesCharger revenuesCharger;
        private TransfersCharger transfersUsCharger;
        public List<ExpensesPerYear> expensesPerYearList { get; set; }
        public List<RevenuesPerYear> revenuesPerYearLIst {  get; set; }
        public List<RevenuesVsExpenses> expensesVsRevenuesList { get; set; }
        public List<TransfersUsPerYear> transfersUsPerYearList { get; set; }

        public ObservableCollection<Brush> ColorModel { get; set; }

        public async Task LoadExpensesPerYear()
        {
            expensesPerYearList = await expensesCharger.LoadExpensesPerYear();
            expensesPerYearList = expensesPerYearList.OrderBy(y => y.Year).ToList();
            OnPropertyChanged(nameof(expensesPerYearList));
            if (expensesPerYearList != null && revenuesPerYearLIst != null)
            {
                LoadExpensesVSRevenues();
            }
        }

        public async Task LoadRevenuesPerYear()
        {
            revenuesPerYearLIst = await revenuesCharger.LoadRevenuesPerYear();
            revenuesPerYearLIst = revenuesPerYearLIst.OrderBy(y => y.Year).ToList();
            OnPropertyChanged(nameof(revenuesPerYearLIst));
            if(expensesPerYearList != null && revenuesPerYearLIst != null)
            {
                LoadExpensesVSRevenues();
            }
        }

        public void LoadExpensesVSRevenues()
        {
            try
            {
                // Obtener todos los años que aparecen en cualquiera de las dos listas
                var allYears = expensesPerYearList.Select(e => e.Year)
                               .Union(revenuesPerYearLIst.Select(r => r.Year)) // Unir todos los años únicos
                               .Distinct() // Asegurarse de no tener duplicados
                               .OrderBy(y => y); // Ordenar por año

                var revenuesVsExpensesList = allYears
                .Select(year => new RevenuesVsExpenses
                {
                    Year = year,
                    Expenses = expensesPerYearList.FirstOrDefault(e => e.Year == year)?.TotalExpenses ?? 0, // Obtener los gastos para el año, si no existe, asignar 0
                    Revenues = revenuesPerYearLIst.FirstOrDefault(r => r.Year == year)?.TotalRevenues ?? 0  // Obtener los ingresos para el año, si no existe, asignar 0
                })
                .ToList();

                expensesVsRevenuesList = revenuesVsExpensesList;
                OnPropertyChanged(nameof(expensesVsRevenuesList));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task LoadTransfersUsPerYear()
        {
            transfersUsPerYearList = await transfersUsCharger.LoadTransfersUsPerYear();
            transfersUsPerYearList = transfersUsPerYearList.OrderBy(y => y.Year).ToList();
            OnPropertyChanged(nameof(transfersUsPerYearList));
        }
    }
}
