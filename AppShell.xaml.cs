namespace HFPMapp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("EditUser", typeof(Views.Users.EditView));
            Routing.RegisterRoute("CreateUser", typeof(Views.Users.CreateView));
        }
    }
}
