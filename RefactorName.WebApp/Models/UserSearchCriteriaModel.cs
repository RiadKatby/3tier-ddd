using RefactorName.Core;
using MCI.Mvc.Validation.Web;
using System.ComponentModel.DataAnnotations;

namespace RefactorName.Web.Models
{
    public class UserSearchCriteriaModel : SearchCriteria
    {
        [Display(Name = "اسم المستخدم")]
        [StringLength(256, ErrorMessage = "يجب أن لايزيد عن {1} حرف")]
        public string UserName { get; set; }

        [Display(Name = "الاسم الكامل")]
        [StringLength(200, ErrorMessage = "يجب أن لايزيد عن {1} حرف")]
        public string FullName { get; set; }

        [Display(Name = "الصلاحية")]
        public string RoleName { get; set; }

        [StringLength(10, ErrorMessage = "يجب أن لايزيد عن {1} حرف")]
        [RegularExpression(@"^05\d{8}$", ErrorMessage="{0} غير صحيح")]
        [Display(Name = "رقم الجوال")]
        public string Mobile { get; set; }

        [Display(Name = "البريد الإلكتروني")]
        [StringLength(256, ErrorMessage = "يجب أن لايزيد عن {1} حرف")]
        public string Email { get; set; }

        [Display(Name = "التفعيل")]
        public bool? IsActive { get; set; }
    }
}