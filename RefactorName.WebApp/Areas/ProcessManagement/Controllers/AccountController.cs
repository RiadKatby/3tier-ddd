using RefactorName.Core;
using RefactorName.Domain;
using RefactorName.WebApp.Filters;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReafactorName.WebApp;
using RefactorName.WebApp.Controllers;
using RefactorName.Domain.Workflow;
using RefactorName.WebApp.Infrastructure.Security;
using Microsoft.AspNet.Identity;

namespace RefactorName.WebApp.Areas.ProcessManagement.Controllers
{
    [InitializeIdentityMembership]
    [MainAuthorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private AppUserManager _userManager;

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
        public AppUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().Get<AppUserManager>();
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
                return RedirectToAction("index", "ProcessManagement");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(Models.LoginViewModel model, string returnUrl)
        {
            SignInStatus result;
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            
            string username = model.Email.ConvertToEasternArabicNumerals();
            string pass = model.Password.ConvertToEasternArabicNumerals();
            var user = UserService.Obj.FindByName(username);
            if (user == null)
            {
                AddMCIMessage("بيانات الدخول غير صحيحه,  تأكد من اسم المستخدم وكلمه المرور", MCIMessageType.Danger);
                return View(model);
            }

            //check IsActive
            if (!user.IsActive)
            {
                AddMCIMessage("المستخدم غير فعال. الرجاء مراجعة مسؤول النظام لتفعيل المستخدم.", MCIMessageType.Warning, 15);
                return View();
            }

            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true

                result = SignInManager.PasswordSignIn(username, pass, false, shouldLockout: false);
                //User firstUser = new User("Admin@Thiqah.com", true, "000000000", "Admin@Thiqah.com");
                //var result2 = UserManager.Create(firstUser, "P@$$w0rd");

                //result = SignInManager.PasswordSignIn("Admin@Thiqah.com", "P@$$w0rd", false, shouldLockout: false);
            }

            switch (result)
            {
                case SignInStatus.Success:
                    Session["User"] = user;
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction("index","ProcessManagement");

                //case SignInStatus.LockedOut:
                //    return View("Lockout");
                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    AddMCIMessage("بيانات الدخول غير صحيحه,  تأكد من اسم المستخدم وكلمه المرور", MCIMessageType.Danger);
                    return View(model);
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
            if (!Request.IsAuthenticated || Session["User"] == null)
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
        public ActionResult ChangePassword(Models.ChangePasswordViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                int userID = (User.Identity as UserProfileIdentity).GetUserId();
                var user = UserService.Obj.FindById(userID);
                //if (user.IsADUser)
                //    MCIAlert.AddMCIMessage(this, "عفوأ. لا يمكن تعديل كلمة مرور السجل النشط.", MCIMessageType.Danger);
                //else
                {
                    var result = UserService.Obj.ChangePasswordAsync(userID, model.OldPassword, model.NewPassword);
                    if (result.Result.Succeeded)
                    {
                        SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        MCIAlert.AddMCIMessage(this, "تم تغيير كلمة المرور بنجاح", MCIMessageType.Success);
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);
                        return RedirectToAction("index", "ProcessManagement");
                    }
                    //translate known errors
                    if (result.Result.Errors.Any(e => e.Contains("Passwords must have")))
                        AddMCIMessage("كلمة المرور يجب أن تحتوي على الأقل على حرف كبير وحرف صغير ورقم.", MCIMessageType.Danger, 0);

                    else if (result.Result.Errors.Contains("Incorrect password."))
                        MCIAlert.AddMCIMessage(this, "كلمة المرور غير صحيحة.", MCIMessageType.Danger);
                    else
                        MCIAlert.AddMCIMessage(this, "عفوأ. حدث خطأ أثناء تعديل كلمة المرور. الرجاء المحاولة لاحقاً.", MCIMessageType.Danger);
                }
            }
            catch
            {
                MCIAlert.AddMCIMessage(this, "عفوأ. حدث خطأ أثناء تعديل كلمة المرور. الرجاء المحاولة لاحقاً.", MCIMessageType.Danger);
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
        //        MCIAlert.AddMCIMessage(this, "عملية غير شرعية", MCIMessageType.Danger, 8);
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
        //        AddMCIMessage("عملية غير شرعية", MCIMessageType.Danger, 8);

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
        //                AddMCIMessage("عفواً..حدث خطأ أثناء حفظ البيانات. الرجاء المحاولة لاحقاً.", MCIMessageType.Danger, 8);
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
