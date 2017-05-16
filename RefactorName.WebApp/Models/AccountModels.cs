using System.ComponentModel.DataAnnotations;

namespace RefactorName.WebApp.Models
{
    public class LoginModel
    {
        [Display(Name = "اسم المستخدم")]
        [Required(ErrorMessage = "الرجاء إدخال {0}")]
        [StringLength(256, MinimumLength = 1, ErrorMessage = " الرجاء ادخال اسم المتخدم بصيغة صحيحة ")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "الرجاء إدخال {0}")]
        [Display(Name = " كلمة المرور")]
        [StringLength(256, MinimumLength = 2, ErrorMessage = " الرجاء ادخال كلمة المرور بصيغة صحيحة ")]
        public string Password { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "الرجاء إدخال {0}")]
        [Display(Name = " كلمة المرور")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = " الرجاء ادخال كلمة المرور بصيغة صحيحة ")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال {0}")]
        [Display(Name = " كلمة المرور الجديدة")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = " الرجاء ادخال كلمة المرور بصيغة صحيحة ")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال {0}")]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("NewPassword", ErrorMessage = "كلمات المرور المدخلة غير متطابقة.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = " الرجاء ادخال كلمة المرور بصيغة صحيحة ")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangeProfilePictureModel
    {
        public string CurrentImagePath { get; set; }

        [Display(Name = "اختيار صورة")]
        public string NewImagePath { get; set; }
    }

    public class UserProfileModel
    {
        public string Identity { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string UserTypeName { get; set; }

        public bool PasswordChangeEnabled { get; set; }

        public byte[] ProfileImage { get; set; }

        public string ProfileImageUrl { get; set; }

        public byte[] ProfileImageThumb { get; set; }

        public string ProfileImageThumbUrl { get; set; }

        public bool ImageChangeEnabled { get; set; }
    }

}
