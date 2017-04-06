using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace RefactorName.Web
{
    public static class SecurityExtenstions
    {
        public static bool Authenticate(this IPrincipal principal, string userName, string password)
        {
            UserProfilePrincipal userProfilePrincipal = principal as UserProfilePrincipal;
            return userProfilePrincipal.Authenticate(userName, password);
        }
    }
}