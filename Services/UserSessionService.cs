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
        public User? CurrentUser { get; set; }

        public bool IsUserLoggedIn => CurrentUser != null;

        public void ClearSession()
        {
            CurrentUser = null;
        }
    }
}
