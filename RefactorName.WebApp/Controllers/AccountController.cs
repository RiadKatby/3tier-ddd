using RefactorName.Core;
using RefactorName.Domain;
using RefactorName.WebApp.Filters;
using RefactorName.WebApp.Helpers;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RefactorName.WebApp.Controllers
{
    [InitializeIdentityMembership]
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (UserLogedin())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(Models.LoginModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            string username = model.UserName.ConvertToEasternArabicNumerals();
            string pass = model.Password.ConvertToEasternArabicNumerals();
            var user = UserService.Obj.FindByName(username);
            if (user == null)
            {
                return View(model)
                    .WithDangerSnackbar("بيانات الدخول غير صحيحه,  تأكد من اسم المستخدم وكلمه المرور");
            }

            //check IsActive
            if (!user.IsActive)
            {
                return View()
                    .WithWarningSnackbar("المستخدم غير فعال. الرجاء مراجعة مسؤول النظام لتفعيل المستخدم.");
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = SignInManager.PasswordSignIn(username, pass, false, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction("index", "Home");

                //case SignInStatus.LockedOut:
                //    return View("Lockout");
                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    return View(model)
                        .WithDangerSnackbar("بيانات الدخول غير صحيحه,  تأكد من اسم المستخدم وكلمه المرور");
            }
        }

        public ActionResult Logout()
        {
            //Abandon will call session_end which will clear the session. this will allow as to access session variables there
            Session.Abandon();
            SignInManager.AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        private bool UserLogedin()
        {
            if (!Request.IsAuthenticated)
            {
                Session.Abandon();
                return false;
            }
            return true;
        }

        public ActionResult ChangePassword(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(Models.ChangePasswordModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                int userID = (User.Identity as UserProfileIdentity).GetUserId();
                var result = UserManager.ChangePasswordAsync(userID, model.OldPassword, model.NewPassword);
                if (result.Result.Succeeded)
                {
                    var user = UserManager.FindByIdAsync(userID);
                    if (user.Result != null)
                    {
                        SignInManager.SignInAsync(user.Result, isPersistent: false, rememberBrowser: false);
                    }
                    ShowSuccessSnackbar("تم تغيير كلمة المرور بنجاح");
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction("index", "Home");
                }
                else if (result.Result.Errors.Contains("Incorrect password."))
                    ShowDangerSnackbar("كلمة المرور غير صحيحة.");
                else
                    ShowDangerSnackbar("عفوأ. حدث خطأ أثناء تعديل كلمة المرور. الرجاء المحاولة لاحقاً.");
            }
            catch
            {
                ShowDangerSnackbar("عفوأ. حدث خطأ أثناء تعديل كلمة المرور. الرجاء المحاولة لاحقاً.");
            }
            return View(model);
            
        }

        #region Profile Picture
        //[HttpGet]
        //public ActionResult ChangeProfilePic(string ReturnUrl)
        //{
        //    var userProfile = Session["UserProfile"] as UserProfileModel;
        //    //make sure change Profile picture enabled
        //    if (!userProfile.ImageChangeEnabled)
        //    {
        //        ShowDangerSnackbar("عملية غير شرعية");
        //        if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
        //            return Redirect(ReturnUrl);
        //        return RedirectToAction("index", "Home");
        //    }

        //    //create the model
        //    var model = new ChangeProfilePictureModel();
        //    if (userProfile.ProfileImage != null) model.CurrentImagePath = Url.Content(Util.ByteArrayToFileInTempFolder(userProfile.ProfileImage, ".jpeg"));
        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ChangeProfilePic(ChangeProfilePictureModel model, string ReturnUrl, string action)
        //{
        //    var userProfile = Session["UserProfile"] as UserProfileModel;
        //    //make sure change Profile picture enabled
        //    if (!userProfile.ImageChangeEnabled)
        //        ShowDangerSnackbar("عملية غير شرعية");

        //    else if (action == "delete")
        //    {
        //        //TODO: remove profile pic from db 

        //        //remove from session
        //        userProfile.ProfileImage = null;
        //        userProfile.ProfileImageThumb = null;
        //        userProfile.ProfileImageThumbUrl = "";
        //        userProfile.ProfileImageUrl = "";
        //    }
        //    else if (action == "save")
        //    {
        //        if (!string.IsNullOrEmpty(model.NewImagePath))
        //        {
        //            byte[] newImage = Util.GetArrayFromFile(Server.MapPath(model.NewImagePath));
        //            try
        //            {
        //                //TODO: update profile pic in db 
        //            }
        //            catch
        //            {
        //                ShowDangerSnackbar("عفواً..حدث خطأ أثناء حفظ البيانات. الرجاء المحاولة لاحقاً.");
        //            }

        //            //update user profile
        //            userProfile.ProfileImage = newImage;
        //            userProfile.ProfileImageThumbUrl = Util.getProfilePhoto(userProfile.ProfileImage);
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
        //        return Redirect(ReturnUrl);
        //    return RedirectToAction("index", "Home");
        //}

        #endregion
    }
}
