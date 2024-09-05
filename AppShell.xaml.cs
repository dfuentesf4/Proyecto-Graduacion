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

        }
    }
}
