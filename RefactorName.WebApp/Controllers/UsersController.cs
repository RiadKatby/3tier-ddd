using RefactorName.Core;
using RefactorName.Domain;
using RefactorName.Web.Filters;
using RefactorName.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using RefactorName.Web.Infrastructure.Encryption;

namespace RefactorName.Web.Controllers
{    
    public class UsersController : BaseController
    {
        int pageSize = 10;
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
                AddMCIMessage("عفواً. حدث خطأ أثناء جلب البيانات. الرجاء المحاولة لاحقاً.", MCIMessageType.Danger, 10);
                return View();
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
                return JsonErrorMessage("عفواً. حدث خطأ أثناء جلب البيانات. الرجاء المحاولة لاحقاً.");
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
                return RedirectwithMCIMessage(Url.Action("Index"), "عفواً. حدث خطأ أثناء جلب البيانات. الرجاء المحاولة لاحقاً.", MCIMessageType.Danger, 10);
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
                        AddMCIMessage(result.Exception.InnerException.Message, MCIMessageType.Danger, 0);
                    else
                        AddMCIMessage(result.Exception.Message, MCIMessageType.Danger, 0);
                    model.FillDDLs();
                    return View(model);
                }
                else if (result.Result.Errors.Any())
                {
                    //translate known errors
                    if (result.Result.Errors.Any(e => e.Contains("is already taken.")))
                        AddMCIMessage("عفواً. المستخدم موجود مسبقاً. الرجاء اختيار اسم آخر.", MCIMessageType.Danger, 0);
                    else
                        AddMCIMessage(string.Join(",", result.Result.Errors), MCIMessageType.Danger, 0);

                    model.FillDDLs();
                    return View(model);
                }

                return RedirectwithMCIMessage(Url.Action("Index"), "تم إضافة المستخدم بنجاح.", MCIMessageType.Success);
            }
            catch (ValidationException valEx)
            {
                valEx.PopulateIn(ModelState);
                model.FillDDLs();
                return View(model);
            }
            catch (BusinessRuleException buzRuleEx)
            {
                AddMCIMessage(buzRuleEx.Message, MCIMessageType.Danger, 10);
                model.FillDDLs();
                return View(model);
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString());
                AddMCIMessage(repEx.Message, MCIMessageType.Danger, 10);
                model.FillDDLs();
                return View(model);
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
                return RedirectwithMCIMessage(Url.Action("Index"), "عفواً. حدث خطأ أثناء جلب البيانات. الرجاء المحاولة لاحقاً.", MCIMessageType.Danger, 10);
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
                    return RedirectwithMCIMessage(Url.Action("Index"), "تم حفظ البيانات بنجاح.", MCIMessageType.Success);
                else
                {
                    AddMCIMessage("عفواً. حصل خطأ أثناء حفظ البيانات. الرجاء المحاولة لاحقاً.", MCIMessageType.Danger, 10);
                    return View(user.ToModel().ToEditModel());
                }
            }
            catch (ValidationException valEx)
            {
                valEx.PopulateIn(ModelState);
                return View(user.ToModel().ToEditModel());
            }
            catch (BusinessRuleException buzRuleEx)
            {
                AddMCIMessage(buzRuleEx.Message, MCIMessageType.Danger, 10);
                return View(user.ToModel().ToEditModel());
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString());
                AddMCIMessage(repEx.Message, MCIMessageType.Danger, 10);
                return View(user.ToModel().ToEditModel());
            }
        }

        [AllowAnonymous]
        public ActionResult HashPassword()
        {
            return View();
        }
    }
}