using HFPMapp.ViewModels.Reports.Accounting;

namespace HFPMapp.Views.Reports.Accounting;

public partial class AccountingReports : ContentPage
{
	public AccountingReports(AccountingReportsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}