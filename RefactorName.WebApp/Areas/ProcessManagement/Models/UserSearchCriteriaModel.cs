using RefactorName.Core;
using MCI.Mvc.Validation.Web;
using System.ComponentModel.DataAnnotations;

namespace RefactorName.WebApp
{
    public class UserSearchCriteriaModel : SearchCriteria
    {
        [Display(Name = "User Name")]
        [StringLength(256, ErrorMessage = "It should be no more than 256 characters")]
        public string UserName { get; set; }

        [Display(Name = "Full Name")]
        [StringLength(200, ErrorMessage = "It should be no more than 200 characters")]
        public string FullName { get; set; }


        //[Display(Name = "Priviledge")]
        //public string RoleName { get; set; }


        [StringLength(10, ErrorMessage = "It should be no more than {1} character")]
        [RegularExpression(@"^05\d{8}$", ErrorMessage = "Not Correct {0}")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        [StringLength(256, ErrorMessage = "It should be no more than {1} character")]

        public string Email { get; set; }

        [Display(Name = "Activity")]
        public bool? IsActive { get; set; }

        [Display(Name = "Group Name")]
        public int? GroupId { get; set; }

        public Group Group { get; set; }

        //[Display(Name = "IsADUser")]
        //public bool? IsADUser { get; set; }

    }
}