using RefactorName.Core;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace RefactorName.WebApp
{
    public class UserProfileIdentity : User, IIdentity
    {
        private ClaimsIdentity originalIdentity;

        public UserProfileIdentity(ClaimsIdentity originalIdentity, User userProfile)
        {
            this.originalIdentity = originalIdentity;
            this.CopyFrom(userProfile);
        }

        public string GetClaimValue(string claimType)
        {
            var claim = originalIdentity.FindFirst(claimType);
            return claim == null ? "" : claim.Value;
        }

        public int GetUserId()
        {
            int result = 0;
            if (originalIdentity != null)
            {
                // find the NameIdentifier claim
                var userIdClaim = originalIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    result = int.Parse(userIdClaim.Value);
                }
            }
            return result;
        }

        #region IIdentity Members

        public string AuthenticationType
        {
            get { return originalIdentity.AuthenticationType; }
        }

        public bool IsAuthenticated
        {
            get { return originalIdentity.IsAuthenticated; }
        }

        public string Name
        {
            get { return originalIdentity.Name; }
        }


        #endregion

        private void CopyFrom(Core.User userProfile)
        {
            //this.CreatedAt = userProfile.CreatedAt;
            //this.CreatedBy = userProfile.CreatedBy;
            //this.CreatedByID = userProfile.CreatedByID;
            //this.IsActive = userProfile.IsActive;
            // this.Password = userProfile.Password;
            //this.UpdatedAt = userProfile.UpdatedAt;
            //this.UpdatedBy = userProfile.UpdatedBy;
            //this.UpdatedByID = userProfile.UpdatedByID;
            this.UserName = userProfile.UserName;
            //this.FullName = userProfile.FullName;
            //this.Id = userProfile.Id;
            this.Email = userProfile.Email;
            //this.Mobile = userProfile.Mobile;
            this.PhoneNumber = userProfile.PhoneNumber;
        }
    }
}