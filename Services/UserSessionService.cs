using HFPMapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Services
{
    public class UserSessionService
    {
        private static User? _currentUser;

        public static User? CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnCurrentUserChanged();
            }
        }

        public static AppShell AppShell { get; set; }

        public bool IsUserLoggedIn => CurrentUser != null;

        public static void ClearSession()
        {
            CurrentUser = null;
        }

        private static void OnCurrentUserChanged()
        {
            AppShell.ApplyUserPrivileges();
        }
    }
}
