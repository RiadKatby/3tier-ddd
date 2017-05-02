using RefactorName.Core;
using RefactorName.Domain;
using RefactorName.WebApp.Filters;
using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using RefactorName.WebApp.Controllers;
using RefactorName.Domain.Workflow;
using RefactorName.Core.SearchEntities;

namespace RefactorName.WebApp.Areas.ProcessManagement.Controllers
{
    public class UserController : BaseController
    {
        // GET: Users
        int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString());

        public UserSearchCriteriaModel UserSearchCriteriaModelStored
        {
            get { return (TempData["UserSearchCriteriaModel"] as UserSearchCriteriaModel) ?? new UserSearchCriteriaModel { PageSize = pageSize }; }
            set { TempData["UserSearchCriteriaModel"] = value; }
        }

        private ApplicationSignInManager _signInManager;
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


        // GET: Users
        //[MainAuthorize(Roles = RoleNames.Admins)]
        public ActionResult Index(bool ls = false, int groupId = 0) //ls is true to load last results
        {
            try
            {
                var loadLastResults = ls || IsRefresh();
                UserIndexModel model = new UserIndexModel
                {
                    userSearchCriteriaModel = loadLastResults ? UserSearchCriteriaModelStored : new UserSearchCriteriaModel()
                };
                if (groupId != 0)
                    model.userSearchCriteriaModel.GroupId = groupId;
                model.Items = CreateUsersList(model.userSearchCriteriaModel);

                FillDropDownLists();

                return View(model.FillDDLsWithUserPermissionOnly());
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString(), repEx.StackTrace);
                AddMCIMessage("عفواً. حدث خطأ أثناء جلب البيانات. الرجاء المحاولة لاحقاً.", MCIMessageType.Danger, 10);
                return View();
            }
        }

        [AjaxOnly]
        //[MainAuthorize(Roles = RoleNames.Admins)]
        public ActionResult GetUsersList(UserIndexModel model, int? page, string sort, string sortdir)
        {
            try
            {
                if (page.HasValue || !string.IsNullOrEmpty(sort))  //paging or sorting requested
                {
                    model.userSearchCriteriaModel = UserSearchCriteriaModelStored;  //take stored criteria                
                    //update paging sorting properties
                    model.userSearchCriteriaModel.PageNumber = page ?? 1;
                    model.userSearchCriteriaModel.Sort = sort;
                    model.userSearchCriteriaModel.SortDirection = sortdir;
                }
                model.userSearchCriteriaModel.PageSize = pageSize;

                var result = CreateUsersList(model.userSearchCriteriaModel);

                return PartialView("_UsersGrid", result);
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString(), repEx.StackTrace);
                return JsonErrorMessage("عفواً. حدث خطأ أثناء جلب البيانات. الرجاء المحاولة لاحقاً.");
            }
        }

        private WebGridList<UserModel> CreateUsersList(UserSearchCriteriaModel userSearchCriteriaModel)
        {
            userSearchCriteriaModel = userSearchCriteriaModel ?? new UserSearchCriteriaModel { PageSize = pageSize };

            UserSearchCriteria searchCriteria = userSearchCriteriaModel.ToEntity();
            IQueryResult<User> queryResult = UserService.Obj.FindUsers(searchCriteria);

            //update stored criteria
            UserSearchCriteriaModelStored = userSearchCriteriaModel;

            return queryResult.ToWebGridListModel();
        }

        //[MainAuthorize(Roles = RoleNames.Admins)]
        public ActionResult AddUser()
        {
            try
            {
                UserAddModel model = new UserAddModel();
                return View("AddUser", model.FillDDLsWithUsersPermissionOnly());
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString(), repEx.StackTrace);
                return RedirectwithMCIMessage(Url.Action("Index"), "عفواً. حدث خطأ أثناء جلب البيانات. الرجاء المحاولة لاحقاً.", MCIMessageType.Danger, 10);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[MainAuthorize(Roles = RoleNames.Admins)]
        public ActionResult AddUser(UserAddModel model)
        {
            try
            {
                //if (model.IsADUser)
                //{
                ModelState["Password"].Errors.Clear();
                ModelState["PasswordConfirm"].Errors.Clear();
                //}

                if (!ModelState.IsValid)
                    return View(model.FillDDLsWithUsersPermissionOnly());

                User newUser = new User(model.FullName, model.IsActive, model.PhoneNumber, model.Email);
                newUser.UpdateRoles(RoleService.Obj.GetByNames(model.Roles).ToList());

                //var result = model.IsADUser ? UserService.Obj.CreateAsync(newUser) : UserService.Obj.CreateAsync(newUser, model.Password);
                var result = UserService.Obj.CreateAsync(newUser, model.Password);

                if (result.Exception != null)
                {
                    if (result.Exception.InnerException != null)
                        AddMCIMessage(result.Exception.InnerException.Message, MCIMessageType.Danger, 0);
                    else
                        AddMCIMessage(result.Exception.Message, MCIMessageType.Danger, 0);
                    return View(model.FillDDLsWithUsersPermissionOnly());
                }
                else if (result.Result.Errors.Any())
                {
                    //translate known errors
                    if (result.Result.Errors.Any(e => e.Contains("Passwords must have")))
                        AddMCIMessage("كلمة المرور يجب أن تحتوي على الأقل على حرف كبير وحرف صغير ورقم.", MCIMessageType.Danger, 0);

                    if (result.Result.Errors.Any(e => e.Contains("is already taken.")))
                        AddMCIMessage("عفواً. المستخدم موجود مسبقاً. الرجاء اختيار اسم آخر.", MCIMessageType.Danger, 0);
                    else
                        AddMCIMessage(string.Join(",", result.Result.Errors), MCIMessageType.Danger, 0);

                    return View(model.FillDDLsWithUsersPermissionOnly());
                }

                return RedirectwithMCIMessage(Url.Action("Index", new { ls = true }), "Users Added Successfully", MCIMessageType.Success);
            }
            catch (ValidationException)
            {
                //valEx.PopulateIn(ModelState);
                return View(model.FillDDLsWithUsersPermissionOnly());
            }
            catch (BusinessRuleException buzRuleEx)
            {
                AddMCIMessage(buzRuleEx.Message, MCIMessageType.Danger, 10);
                return View(model.FillDDLsWithUsersPermissionOnly());
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString(), repEx.StackTrace);
                AddMCIMessage(repEx.Message, MCIMessageType.Danger, 10);
                return View(model.FillDDLsWithUsersPermissionOnly());
            }
        }

        //[AjaxOnly]
        //[HttpPost]
        //public ActionResult ValidateUser(string userName)
        //{
        //    try
        //    {
        //        var user = SignInManager.ActiveDirectoryUserGetInfo(userName);

        //        if (user == null)
        //            return JsonErrorMessage("المستخدم غير موجود في السجل النشط");

        //        return Json(new
        //        {
        //            user.DisplayName,
        //            user.Email,
        //            user.Mobile
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (RepositoryException repEx)
        //    {
        //        Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString(), repEx.StackTrace);
        //        return JsonErrorMessage(repEx.Message);
        //    }
        //}

        [AjaxOnly]
        [HttpPost]
        public ActionResult ActivateUser(int ID, int active)
        {
            var user = UserService.Obj.FindById(ID);
            if (active == 0)
                user.Deactivate();
            else
                user.Activate();

            var updatedUser = UserService.Obj.Update(user).ToModel();
            var result = CreateUsersList(new UserSearchCriteriaModel());

            return PartialView("_UsersGrid", result);
            //return PartialView("_UsersGrid");
        }

        public void FillDropDownLists()
        {
            var groups = GroupService.Obj.GetAllGroups().Items.ToList().OrderBy(x => x.Name);
            ViewBag.Groups = new SelectList(groups, "GroupId", "Name");
        }

        //[MainAuthorize(Roles = RoleNames.UsersEdit)]
        //[EncryptedActionParameter]
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
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString(), repEx.StackTrace);
                return RedirectwithMCIMessage(Url.Action("Index"), "عفواً. حدث خطأ أثناء جلب البيانات. الرجاء المحاولة لاحقاً.", MCIMessageType.Danger, 10);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[MainAuthorize(Roles = RoleNames.UsersEdit)]
        public ActionResult EditUser(UserEditModel model, string command)
        {
            User user = null;
            try
            {
                //get user
                user = UserService.Obj.FindByName(model.Email);

                if (!ModelState.IsValid)
                    return View(model.FillDDLs());

                user.Update(model.FullName, model.IsActive, model.PhoneNumber, model.Email)
                    .UpdateRoles(RoleService.Obj.GetByNames(model.Roles).ToList());

                var updatedUser = UserService.Obj.Update(user);

                if (updatedUser != null)
                    return RedirectwithMCIMessage(Url.Action("Index", new { ls = true }), "تم حفظ البيانات بنجاح.", MCIMessageType.Success);
                else
                {
                    AddMCIMessage("عفواً. حصل خطأ أثناء حفظ البيانات. الرجاء المحاولة لاحقاً.", MCIMessageType.Danger, 10);
                    return View(model.FillDDLs());
                }
            }
            catch (ValidationException /*valEx*/)
            {
                //valEx.PopulateIn(ModelState);
                return View(model.FillDDLs());
            }
            catch (BusinessRuleException buzRuleEx)
            {
                AddMCIMessage(buzRuleEx.Message, MCIMessageType.Danger, 10);
                return View(model.FillDDLs());
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString(), repEx.StackTrace);
                AddMCIMessage(repEx.Message, MCIMessageType.Danger, 10);
                return View(model.FillDDLs());
            }
        }
    }
}