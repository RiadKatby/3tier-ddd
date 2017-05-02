using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;
using RefactorName.Core;
using RefactorName.Domain;
using System.Security.Claims;

namespace RefactorName.WebApp
{
    public class UserProfilePrincipal : IPrincipal
    {
        private ClaimsPrincipal originalPrincipal;

        public UserProfilePrincipal(ClaimsPrincipal originalPrincipal, ClaimsIdentity originalIdentity, User userProfile)
        {
            this.originalPrincipal = originalPrincipal;
            this.Identity = new UserProfileIdentity(originalIdentity, userProfile);
        }

        public bool IsInRoles(string roles)
        {
            foreach (var roleTocheck in roles.Trim().Split(','))
                if (IsInRole(roleTocheck.Trim()))
                    return true;
            return false;
        }


        #region IPrincipal Members

        /// <summary>
        /// Gets a value indicating whether the user represented by the System.Web.Security.RolePrincipal is in the specified role.
        /// </summary>
        /// <param name="role">The role to search for.</param>
        /// <returns>true if user represented by the System.Web.Security.RolePrincipal is in the specified role; otherwise, false.</returns>
        public bool IsInRole(string role)
        {
            if (originalPrincipal.IsInRole(role) || originalPrincipal.IsInRole("/"))
                return true;
            else if (!role.Contains('/'))
                return false;
            else
            {
                var newRoleToCheck = role.Substring(0, role.LastIndexOf('/'));
                return IsInRole(newRoleToCheck);
            }
        }

        public IIdentity Identity { get; private set; }

        #endregion
    }
}