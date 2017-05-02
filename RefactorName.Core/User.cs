using Microsoft.AspNet.Identity;
using RefactorName.Core;
using RefactorName.GraphDiff.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    [Serializable]
    public class User : IdentityUser
    {
        //public int UserId { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("FullName", this.FullName ?? ""));
            userIdentity.AddClaim(new Claim("Email", this.Email ?? ""));

            return userIdentity;
        }
        #region IUser<int> interface members


        [DefaultValue(false)]
        public bool IsActive { get; protected set; }




        //public string UserName
        //{
        //    get { return Username; }
        //    set { Username = value; }
        //}

        #endregion

        

        //public override string UserName { get; set; }

        public string FullName { get; protected set; }
        public override string Email { get; set; }
        public override string PasswordHash { get; set; }
        public override string PhoneNumber { get; set; }


        [Associated]
        public IList<Group> Groups { get; set; }


        [Associated]
        public IList<Process> Processes { get; set; }

        public User(int id)
            : this()
        {
            this.Id = id;
        }

        public User(string fullName,bool isActive,string phoneNumber,string email)
        {
            this.UserName = email;
            this.FullName = fullName;
            this.IsActive = isActive;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
        }

        public User()
        {

        }

        public void Deactivate()
        {
            this.IsActive = false;
        }

        public void Activate()
        {
            this.IsActive = true;
        }

        public User Update(string fullName, bool isActive, string phoneNumber, string email)
        {
            this.FullName = fullName;
            this.IsActive = isActive;
            this.PhoneNumber = phoneNumber;
            this.Email = email;

            //this.UpdatedAt = DateTime.Now;
            //this.UpdatedBy = Thread.CurrentPrincipal.Identity as User;
            //this.UpdatedByID = this.UpdatedBy.Id;

            return this;
        }
        public User UpdateRoles(List<IdentityRole> newRoles)
        {
            this.Roles = newRoles;
            return this;
        }
    }
}
