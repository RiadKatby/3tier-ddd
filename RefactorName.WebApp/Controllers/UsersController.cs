using RefactorName.Core;
using RefactorName.Domain;
using RefactorName.WebApp.Filters;
using RefactorName.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using RefactorName.WebApp.Infrastructure.Encryption;
using RefactorName.WebApp.Helpers;

namespace RefactorName.WebApp.Controllers
{
    public class UsersController : BaseController
    {
        readonly int pageSize = 10;
        private ApplicationUserManager _userManager;
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


        public UserSearchCriteriaModel UserSearchCriteriaModelStored
        {
            get { return (TempData["UserSearchCriteriaModel"] as UserSearchCriteriaModel) ?? new UserSearchCriteriaModel { PageSize = pageSize }; }
            set { TempData["UserSearchCriteriaModel"] = value; }
        }

        // GET: Users
        [Authorize(Roles = RoleNames.UsersView + "," + RoleNames.UsersAdd + "," + RoleNames.UsersEdit)]
        public ActionResult Index()
        {
            try
            {
                UserIndexModel model = new UserIndexModel
                {
                    userSearchCriteriaModel = UserSearchCriteriaModelStored
                };

                model.Items = CreateUsersList(model.userSearchCriteriaModel);

                return View(model.FillDDLs());
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString());

                return View()
                    .WithDangerSnackbar("عفواً. حدث خطأ أثناء جلب البيانات. الرجاء المحاولة لاحقاً.");
            }
        }

        [Ajax]
        [Authorize(Roles = RoleNames.UsersView + "," + RoleNames.UsersAdd + "," + RoleNames.UsersEdit)]
        public ActionResult GetUsersList(UserIndexModel model, int? page, string sort, SortOrderEnum sortdir)
        {
            try
            {
                if (page.HasValue || !string.IsNullOrEmpty(sort))  //paging or sorting requested
                {
                    model.userSearchCriteriaModel = UserSearchCriteriaModelStored;  //take stored criteria
                    //update paging sorting properties
                    model.userSearchCriteriaModel.PageSize = pageSize;
                    model.userSearchCriteriaModel.PageNumber = page ?? 1;
                    model.userSearchCriteriaModel.Sort = sort;
                    model.userSearchCriteriaModel.SortDirection = sortdir;
                }

                var result = CreateUsersList(model.userSearchCriteriaModel);

                return PartialView("_UsersGrid", result);
            }
            catch (Exception repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString());
                return DangerSnackbar("عفواً. حدث خطأ أثناء جلب البيانات. الرجاء المحاولة لاحقاً.");
            }
        }

        private WebGridList<UserModel> CreateUsersList(UserSearchCriteriaModel userSearchCriteriaModel)
        {
            userSearchCriteriaModel = userSearchCriteriaModel ?? new UserSearchCriteriaModel { PageSize = pageSize };
            UserSearchCriteria searchCriteria = userSearchCriteriaModel.ToEntity();
            IQueryResult<User> queryResult = UserService.Obj.Find(searchCriteria);

            //update stored criteria
            UserSearchCriteriaModelStored = userSearchCriteriaModel;

            return queryResult.ToWebGridListModel();
        }

        [Authorize(Roles = RoleNames.UsersAdd)]
        public ActionResult AddUser()
        {
            try
            {
                UserAddModel model = new UserAddModel();
                model.FillDDLs();
                return View("AddUser", model);
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString());

                return Redirect(Url.Action(nameof(Index)))
                    .WithDangerSnackbar("عفواً. حدث خطأ أثناء جلب البيانات. الرجاء المحاولة لاحقاً.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.UsersAdd)]
        public ActionResult AddUser(UserAddModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.FillDDLs();
                    return View(model);
                }

                User newUser = new User(model.UserName, model.FullName, model.IsActive, model.Mobile, model.Email);
                newUser.UpdateRoles(RoleService.Obj.GetByNames(model.Roles).ToList());

                var result = UserManager.CreateAsync(newUser, model.Password);

                if (result.Exception != null)
                {
                    if (result.Exception.InnerException != null)
                        ShowDangerSnackbar(result.Exception.InnerException.Message);
                    else
                        ShowDangerSnackbar(result.Exception.Message);

                    model.FillDDLs();
                    return View(model);
                }
                else if (result.Result.Errors.Any())
                {
                    //translate known errors
                    if (result.Result.Errors.Any(e => e.Contains("is already taken.")))
                        ShowDangerSnackbar("عفواً. المستخدم موجود مسبقاً. الرجاء اختيار اسم آخر.");
                    else
                        ShowDangerSnackbar(string.Join(",", result.Result.Errors));

                    model.FillDDLs();
                    return View(model);
                }

                return RedirectToAction(nameof(Index))
                    .WithSuccessSnackbar("تم إضافة المستخدم بنجاح.");
            }
            catch (ValidationException valEx)
            {
                valEx.PopulateIn(ModelState);
                model.FillDDLs();
                return View(model);
            }
            catch (BusinessRuleException buzRuleEx)
            {
                model.FillDDLs();

                return View(model)
                    .WithDangerSnackbar(buzRuleEx.Message);
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString());

                model.FillDDLs();

                return View(model)
                    .WithDangerSnackbar(repEx.Message);
            }
        }

        [Authorize(Roles = RoleNames.UsersEdit)]
        [EncryptedActionParameter]
        public ActionResult EditUser(string userName)
        {
            try
            {
                var user = UserService.Obj.FindByName(userName);
                UserEditModel model = user.ToModel().ToEditModel();
                return View(model);
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString());

                return RedirectToAction(nameof(Index))
                    .WithDangerSnackbar("عفواً. حدث خطأ أثناء جلب البيانات. الرجاء المحاولة لاحقاً.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.UsersEdit)]
        public ActionResult EditUser(UserEditModel model, string command)
        {
            User user = null;
            try
            {
                //get user
                user = UserService.Obj.FindByName(model.UserName);

                if (!ModelState.IsValid)
                {
                    return View(user.ToModel().ToEditModel());
                }

                user.Update(model.FullName, model.IsActive, model.Mobile, model.Email)
                    .UpdateRoles(RoleService.Obj.GetByNames(model.Roles).ToList());

                var updatedUser = UserService.Obj.Update(user);

                if (updatedUser != null)
                    return RedirectToAction(nameof(Index))
                        .WithSuccessSnackbar("تم حفظ البيانات بنجاح.");
                else
                    return View(user.ToModel().ToEditModel())
                        .WithDangerSnackbar("عفواً. حصل خطأ أثناء حفظ البيانات. الرجاء المحاولة لاحقاً.");
            }
            catch (ValidationException valEx)
            {
                valEx.PopulateIn(ModelState);
                return View(user.ToModel().ToEditModel());
            }
            catch (BusinessRuleException buzRuleEx)
            {
                return View(user.ToModel().ToEditModel())
                    .WithDangerSnackbar(buzRuleEx.Message);
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString());

                return View(user.ToModel().ToEditModel())
                    .WithDangerSnackbar(repEx.Message);
            }
        }

        [AllowAnonymous]
        public ActionResult HashPassword()
        {
            return View();
        }
    }
}