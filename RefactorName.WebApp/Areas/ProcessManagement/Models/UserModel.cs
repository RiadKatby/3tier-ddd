using System.Linq;
using RefactorName.Core;
using RefactorName.Domain;
using MCI.Mvc.Validation.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using static RefactorName.Core.Enum;

namespace RefactorName.WebApp
{
    public class UserModel
    {
        public int UserID { get; set; }
        public int Id { get; set; }

        //[Display(Name = "User Name")]
        //[Required(ErrorMessage = "Please Enter {0}")]
        //[StringLength(256, ErrorMessage = "It should be no more than 256 characters")]
        //public string UserName { get; set; }

        [Display(Name = "Activity")]
        public bool IsActive { get; set; }

        //[Display(Name = "Active Directory User")]
        //public bool IsADUser { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [StringLength(200, ErrorMessage = "It should be no more than {1} character")]
        //[StringLength(200, ErrorMessage = "يجب أن لايزيد عن {1} حرف")]
        public string FullName { get; set; }


        //[StringLength(10, ErrorMessage = "يجب أن لايزيد عن {1} حرف")]
        [StringLength(10, ErrorMessage = "It should be no more than {1} character")]
        [RegularExpression(@"^05\d{8}$", ErrorMessage = "Not Correct {0}")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please Enter Mobile Number")]
        [StringLength(256, ErrorMessage = "It should be no more than {1} character")]
        [EmailAddress(ErrorMessage = "Not Correct {0}")]
        public string Email { get; set; }

        public string UserName { get
            {
                return this.Email;
            }
            set
            {
                value = this.Email;
            }
        }
        public Status UserStatus { get; set; }
        [Display(Name = "Priviledges")]
        public string[] Roles { get; set; }

        [Display(Name = "Priviledges")]
        public string Priviledges { get; set; }

        public UserModel()
        {

        }
    }

    public class UserEditModel
    {
        public int UserID { get; set; }

        //[Display(Name = "User Name")]
        //[Required(ErrorMessage = "Please Enter {0}")]
        //[StringLength(256, ErrorMessage = "It should be no more than 256 characters")]
        //public string UserName { get; set; }

        [Display(Name = "Activity")]
        public bool IsActive { get; set; }

        //[Display(Name = "Active Directory User")]
        //public bool IsADUser { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [StringLength(200, ErrorMessage = "It should be no more than {1} character")]
        public string FullName { get; set; }


        [StringLength(10, ErrorMessage = "It should be no more than {1} character")]
        [RegularExpression(@"^05\d{8}$", ErrorMessage = "Not Correct {0}")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please Enter Mobile Number")]
        [StringLength(256, ErrorMessage = "It should be no more than {1} character")]
        [EmailAddress(ErrorMessage = "Not Correct {0}")]
        public string Email { get; set; }

        public Status UserStatus { get; set; }
        [Display(Name = "Priviledges")]
        public string[] Roles { get; set; }

        public Dictionary<string, string> RoleNames { get; set; }

        public UserEditModel()
        {
            RoleNames = new Dictionary<string, string>();
        }

        public UserEditModel(UserModel model)
        {
            this.Email = model.Email;
            this.FullName = model.FullName;
            this.IsActive = model.IsActive;
            this.PhoneNumber = model.PhoneNumber;
            this.UserID = model.UserID;
            this.Roles = model.Roles;
            //this.UserName = model.UserName;
        }

        public UserEditModel FillDDLs()
        {
            this.RoleNames = Core.RoleNames.GetRolesWithCaptions();
            return this;
        }

        public UserEditModel FillDDLsWithoutUsersPermission()
        {
            this.RoleNames = Core.RoleNames.GetRolesWithCaptions();
            return this;
        }

        public UserEditModel FillDDLsWithUsersPermissionOnly()
        {
            this.RoleNames = Core.RoleNames.GetRolesWithCaptions();
            return this;
        }
    }

    public class UserAddModel : UserEditModel
    {
        [Display(Name = "Password")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Error")]
        public string Password { get; set; }


        [Display(Name = "Password Confirmation")]
        [Compare("Password", ErrorMessage = "Error")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Error")]
        public string PasswordConfirm { get; set; }

        public UserAddModel() : base() { }

        public UserAddModel(UserModel model)
            : base(model)
        {
        }
    }
}