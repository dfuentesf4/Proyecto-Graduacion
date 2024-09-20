namespace HFPMapp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //Rutas User
            Routing.RegisterRoute("EditUser", typeof(Views.Users.EditView));
            Routing.RegisterRoute("CreateUser", typeof(Views.Users.CreateView));
            //Rutas Donor
            Routing.RegisterRoute("EditDonor", typeof(Views.Donors.EditView));
            Routing.RegisterRoute("CreateDonor", typeof(Views.Donors.CreateView));
            //Rutas Project
            Routing.RegisterRoute("ListProject", typeof(Views.Projects.ProjectsListView));
            Routing.RegisterRoute("EditProject", typeof(Views.Projects.ProjectEditView));
            Routing.RegisterRoute("CreateProject", typeof(Views.Projects.ProyectsCreateView));
            //Rutas Beneficiary
            Routing.RegisterRoute("EditBeneficiary", typeof(Views.Projects.Beneficiaries.EditView));
            Routing.RegisterRoute("CreateBeneficiary", typeof(Views.Projects.Beneficiaries.CreateView));
            Routing.RegisterRoute("ListBeneficiary", typeof(Views.Projects.Beneficiaries.ListView));
            //Rutas Volunteer
            Routing.RegisterRoute("EditVolunteer", typeof(Views.Projects.Volunteers.EditView));
            Routing.RegisterRoute("CreateVolunteer", typeof(Views.Projects.Volunteers.CreateView));
            Routing.RegisterRoute("ListVolunteer", typeof(Views.Projects.Volunteers.ListView));
            //Rutas Reports
            Routing.RegisterRoute("ListReport", typeof(Views.Projects.Reports.ListView));
            Routing.RegisterRoute("CreateReport", typeof(Views.Projects.Reports.CreateView));
            Routing.RegisterRoute("EditReport", typeof(Views.Projects.Reports.EditView));
            //Rutas Activity
            Routing.RegisterRoute("ListActivity", typeof(Views.Projects.Activities.ListView));
            Routing.RegisterRoute("CreateActivity", typeof(Views.Projects.Activities.CreateView));
            Routing.RegisterRoute("EditActivity", typeof(Views.Projects.Activities.EditView));
            //Rutas ExpensesDetail
            Routing.RegisterRoute("ListExpensesDetail", typeof(Views.Accounting.ExpensesDetails.ListView));
            Routing.RegisterRoute("CreateExpensesDetail", typeof(Views.Accounting.ExpensesDetails.CreateView));
            Routing.RegisterRoute("EditExpensesDetail", typeof(Views.Accounting.ExpensesDetails.EditView));
            //Rutas RevenuesDetail
            Routing.RegisterRoute("ListRevenuesDetail", typeof(Views.Accounting.RevenuesDetails.ListView));
            Routing.RegisterRoute("CreateRevenuesDetail", typeof(Views.Accounting.RevenuesDetails.CreateView));
            Routing.RegisterRoute("EditRevenuesDetail", typeof(Views.Accounting.RevenuesDetails.EditView));
            //Rutas Summary
            Routing.RegisterRoute("ListSummary", typeof(Views.Accounting.Summary.ListView));
            Routing.RegisterRoute("CreateSummary", typeof(Views.Accounting.Summary.CreateView));
            Routing.RegisterRoute("EditSummary", typeof(Views.Accounting.Summary.EditView));
            //Rutas PettyCashSummary
            Routing.RegisterRoute("ListPettyCashSummary", typeof(Views.Accounting.PettyCashSummary.ListView));
            Routing.RegisterRoute("CreatePettyCashSummary", typeof(Views.Accounting.PettyCashSummary.CreateView));
            Routing.RegisterRoute("EditPettyCashSummary", typeof(Views.Accounting.PettyCashSummary.EditView));
            //Rutas TransfersFromUS
            Routing.RegisterRoute("ListTransfersFromU", typeof(Views.Accounting.TransfersFromUS.ListView));
            Routing.RegisterRoute("CreateTransfersFromU", typeof(Views.Accounting.TransfersFromUS.CreateView));
            Routing.RegisterRoute("EditTransfersFromU", typeof(Views.Accounting.TransfersFromUS.EditView));
            //Rutas TransfersSummary
            Routing.RegisterRoute("ListTransferSummary", typeof(Views.Accounting.TransfersSummary.ListView));
            Routing.RegisterRoute("CreateTransferSummary", typeof(Views.Accounting.TransfersSummary.CreateView));
            Routing.RegisterRoute("EditTransferSummary", typeof(Views.Accounting.TransfersSummary.EditView));
            //Rutas Bank
            Routing.RegisterRoute("ListBank", typeof(Views.Accounting.Bank.ListView));
            Routing.RegisterRoute("CreateBank", typeof(Views.Accounting.Bank.CreateView));
            Routing.RegisterRoute("EditBank", typeof(Views.Accounting.Bank.EditView));
            //Rutas FolderBank
            Routing.RegisterRoute("ListFolderBank", typeof(Views.Accounting.FolderBank.ListView));
            Routing.RegisterRoute("CreateFolderBank", typeof(Views.Accounting.FolderBank.CreateView));
            Routing.RegisterRoute("EditFolderBank", typeof(Views.Accounting.FolderBank.EditView));
            //Rutas BankSummary
            Routing.RegisterRoute("ListBankSummary", typeof(Views.Accounting.BankSummary.ListView));
            Routing.RegisterRoute("CreateBankSummary", typeof(Views.Accounting.BankSummary.CreateView));
            Routing.RegisterRoute("EditBankSummary", typeof(Views.Accounting.BankSummary.EditView));
            //Rutas BankBook
            Routing.RegisterRoute("ListBankBook", typeof(Views.Accounting.BankBook.ListView));
            Routing.RegisterRoute("CreateBankBook", typeof(Views.Accounting.BankBook.CreateView));
            Routing.RegisterRoute("EditBankBook", typeof(Views.Accounting.BankBook.EditView));


        }
    }
}
