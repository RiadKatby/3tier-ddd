using Newtonsoft.Json.Linq;
using ReafactorName.WebApp;
using RefactorName.Core;
using RefactorName.Core.SearchEntities;
using RefactorName.Domain;
using RefactorName.Domain.Workflow;
using RefactorName.WebApp.Areas.Backend.Models;
using RefactorName.WebApp.Controllers;
using RefactorName.WebApp.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RefactorName.WebApp.Areas.ProcessManagement.Controllers
{
    [MainAuthorize(Roles = RoleNames.SuperAdministrator)]
    public class ProcessManagementController : BaseController
    {

        // GET: Product
        int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString());

        public ProcessSearchCriteriaModel ProcessSearchCriteriaModelStored
        {
            get { return (TempData["ProcessSearchCriteriaModel"] as ProcessSearchCriteriaModel) ?? new ProcessSearchCriteriaModel { PageSize = pageSize }; }
            set { TempData["ProcessSearchCriteriaModel"] = value; }
        }
        //[MainAuthorize(Roles = RoleNames.Admins)]
        public ActionResult Index(bool ls = false) //ls is true to load last results
        {
            try
            {
                var loadLastResults = ls || IsRefresh();
                ProcessIndexModel model = new ProcessIndexModel
                {
                    processSearchCriteriaModel = loadLastResults ? ProcessSearchCriteriaModelStored : new ProcessSearchCriteriaModel()
                };

                model.Items = CreateProcessesList(model.processSearchCriteriaModel);
                return View(model);
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString(), repEx.StackTrace);
                AddMCIMessage("عفواً. حدث خطأ أثناء جلب البيانات. الرجاء المحاولة لاحقاً.", MCIMessageType.Danger, 10);
                return View();
            }
        }

        [AjaxOnly]
        public ActionResult CreateProcessesList(ProcessIndexModel model, int? page, string sort, string sortdir)
        {
            try
            {
                if (page.HasValue || !string.IsNullOrEmpty(sort))  //paging or sorting requested
                {
                    model.processSearchCriteriaModel = ProcessSearchCriteriaModelStored;  //take stored criteria                
                    //update paging sorting properties
                    model.processSearchCriteriaModel.PageNumber = page ?? 1;
                    model.processSearchCriteriaModel.Sort = sort;
                    model.processSearchCriteriaModel.SortDirection = sortdir;
                }
                model.processSearchCriteriaModel.PageSize = pageSize;

                var result = CreateProcessesList(model.processSearchCriteriaModel);

                return PartialView("_ProcessesGrid", result);
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString(), repEx.StackTrace);
                return JsonErrorMessage("عفواً. حدث خطأ أثناء جلب البيانات. الرجاء المحاولة لاحقاً.");
            }
        }

        private WebGridList<ProcessAddModel> CreateProcessesList(ProcessSearchCriteriaModel processSearchCriteriaModel)
        {
            processSearchCriteriaModel = processSearchCriteriaModel ?? new ProcessSearchCriteriaModel { PageSize = pageSize };
            ProcessSearchCriteria searchCriteria = processSearchCriteriaModel.ToEntity();
            IQueryResult<Core.Process> queryResult = ProcessService.Obj.Find(searchCriteria);

            //update stored criteria
            ProcessSearchCriteriaModelStored = processSearchCriteriaModel;

            return queryResult.ToWebGridListModel();
        }

        [AjaxOnly]
        public ActionResult GetProcessesList(ProcessIndexModel model, int? page, string sort, string sortdir)
        {
            try
            {
                if (page.HasValue || !string.IsNullOrEmpty(sort))  //paging or sorting requested
                {
                    model.processSearchCriteriaModel = ProcessSearchCriteriaModelStored;  //take stored criteria                
                    //update paging sorting properties
                    model.processSearchCriteriaModel.PageNumber = page ?? 1;
                    model.processSearchCriteriaModel.Sort = sort;
                    model.processSearchCriteriaModel.SortDirection = sortdir;
                }
                model.processSearchCriteriaModel.PageSize = pageSize;

                var result = CreateProcessesList(model.processSearchCriteriaModel);

                return PartialView("_ProcessesGrid", result);
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString(), repEx.StackTrace);
                return JsonErrorMessage("عفواً. حدث خطأ أثناء جلب البيانات. الرجاء المحاولة لاحقاً.");
            }
        }
        public ActionResult Delete(int id = 0)
        {
            Core.Process tempProcess = ProcessService.Obj.FindByIdToDelete(id);

            ProcessService.Obj.Delete(tempProcess);

            IEnumerable<ProcessAddModel> models = ProcessService.Obj.GetAllProcesses().ToModels();

            return RedirectwithMCIMessage(Url.Action("Index", new { isDeleted = true }), "Deleted Success");
        }
        // GET: ProcessManagement/ProcessManagement
        public ActionResult ManageProcess(int id = 0)
        {
            try
            {
                ProcessAddModel model = new ProcessAddModel();
                if (id != 0)
                {
                    Core.Process tempProcess = ProcessService.Obj.FindById(id);
                    if (tempProcess == null)
                        return Index();
                    model = tempProcess.ToModel();
                }
                CreateUsersList();
                ViewBag.ProcessId = model.ProcessId;
                return View("ManageProcess", model);
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString(), repEx.StackTrace);
                return null;
            }
        }

        [AjaxOnly]
        public ActionResult CreateProcess(ProcessAddModel[] model)
        {
            try
            {

                Core.Process tempProcess = ProcessService.Obj.FindById(model[0].ProcessId);
                List<User> processAdmins = tempProcess.Users.ToList();
                if (model[0].UserNames == null)
                {
                    model[0].UserNames = new Dictionary<string, string>();
                }
                foreach (User admin in processAdmins)
                {
                    bool isDeleted = true;
                    foreach (KeyValuePair<string, string> tempUser in model[0].UserNames)
                    {
                        int id = int.Parse(tempUser.Key);
                        if (admin.Id == id)
                        {
                            isDeleted = false;
                            break;
                        }
                    }
                    if (isDeleted)
                        tempProcess.RemoveAdmin(admin);
                }
                foreach (KeyValuePair<string, string> tempUser in model[0].UserNames)
                {
                    int id = int.Parse(tempUser.Key);
                    User newAdmin = UserService.Obj.FindById(id);
                    tempProcess.AddAdmin(newAdmin);
                }
                tempProcess.UpdateName(model[0].Name);
                tempProcess = ProcessService.Obj.Update(tempProcess);

                List<GroupAddModel> groupModels = tempProcess.Groups.ToList().ToModels();
                CreateUsersList();
                ViewBag.ProcessId = tempProcess.ProcessId;
                ViewBag.Message = "Success";
                ViewBag.Style = "inline-block";
                AddMCIMessage("Success", MCIMessageType.Info);
                return PartialView("_AddGroup", groupModels);

            }
            catch (Exception)
            {
                CreateUsersList();
                ViewBag.ProcessId = model[0].ProcessId;
                var tempModel = new ProcessAddModel();
                return View(tempModel);
            }
        }

        [AjaxOnly]
        public ActionResult CreateActivities(ActivityAddModel[] models)
        {
            try
            {
                Core.Process tempProcess = ProcessService.Obj.FindById(models[0].ProcessId);

                var isValid = true;
                #region Remove Deleted Items   
                List<Activity> processActivities = tempProcess.Activities.ToList();
                foreach (Activity activity in processActivities)
                {
                    bool isActivityDeleted = true;
                    foreach (ActivityAddModel activityAddModel in models)
                    {
                        if (activity.ActivityId == activityAddModel.ActivityId)
                        {
                            isActivityDeleted = false;
                            break;
                        }
                    }
                    if (isActivityDeleted)
                    {
                        bool hasActivityTarget = ActivityTargetService.Obj.FindByActivityId(activity.ActivityId).TotalCount > 0 ? true : false;
                        if (activity.States.Count == 0 && activity.Transitions.Count == 0 && !hasActivityTarget)
                        {
                            var tempActivity = ActivityService.Obj.FindById(activity.ActivityId);
                            ActivityService.Obj.Delete(tempActivity);
                        }
                        else
                        {
                            isValid = false;
                        }
                    }
                }
                #endregion
                tempProcess = ProcessService.Obj.FindById(models[0].ProcessId);

                foreach (ActivityAddModel activityAddModel in models)
                {
                    var tempActivity = (from a in tempProcess.Activities where a.ActivityId == activityAddModel.ActivityId select a).FirstOrDefault();
                    if (tempActivity == null)
                        tempActivity = new Activity();

                    var activityTypeResult = ActivityTypeService.Obj.FindById(activityAddModel.ActivityTypeID);
                    tempActivity.Update(activityAddModel.Name, activityAddModel.Description, activityTypeResult);

                    tempProcess = tempProcess.AddActivity(tempActivity);

                    tempProcess = ProcessService.Obj.Update(tempProcess);
                }
                if (isValid)
                {

                    List<ActivityTargetAddModel> activityTargetModels = ActivityTargetService.Obj.GetActivityTargetesByProcessId(models[0].ProcessId).ToModels();
                    FillCheckBoxLists(models[0].ProcessId);
                    ViewBag.ProcessId = models[0].ProcessId;
                    ViewBag.Message = "Success";
                    ViewBag.Style = "inline-block";
                    return PartialView("_AddActivityTarget", activityTargetModels);
                }
                else
                {
                    ViewBag.Message = "There is Related Data";
                    ViewBag.Style = "inline-block";
                    return BackToActivities(models[0].ProcessId.ToString());
                }
            }
            catch(Exception /*ex*/)
            {
                return BackToActivities(models[0].ProcessId.ToString());
            }
        }

        [AjaxOnly]
        public ActionResult CreateActions(ActionAddModel[] models)
        {
            try
            {
                Core.Process tempProcess = ProcessService.Obj.FindById(models[0].ProcessId);
                var isValid = true;
                #region Remove Deleted Items   
                List<Core.Action> processActions = tempProcess.Actions.ToList();
                foreach (Core.Action action in processActions)
                {
                    bool isActionDeleted = true;
                    foreach (ActionAddModel actionAddModel in models)
                    {
                        if (action.ActionId == actionAddModel.ActionId)
                        {
                            isActionDeleted = false;
                            break;
                        }
                    }
                    if (isActionDeleted)
                    {
                        bool hasActionTarget = ActionTargetService.Obj.FindByActionId(action.ActionId).TotalCount > 0 ? true : false;
                        if (action.Transitions.Count == 0 && !hasActionTarget)
                        {
                            var tempAction = ActionService.Obj.FindById(action.ActionId);
                            ActionService.Obj.Delete(tempAction);
                        }
                        else
                        {
                            isValid = false;
                        }
                    }
                }
                #endregion


                foreach (ActionAddModel actionAddModel in models)
                {
                    var tempAction = (from a in tempProcess.Actions where a.ActionId == actionAddModel.ActionId select a).FirstOrDefault();
                    if (tempAction == null)
                        tempAction = new Core.Action();

                    var actionTypeResult = ActionTypeService.Obj.FindById(actionAddModel.ActionTypeID);
                    tempAction.Update(actionAddModel.Name, actionAddModel.Description, actionTypeResult);

                    tempProcess = tempProcess.AddAction(tempAction);

                    tempProcess = ProcessService.Obj.Update(tempProcess);
                }

                if (isValid)
                {
                    List<ActionTargetAddModel> actionTargetModels = ActionTargetService.Obj.GetActionTargetesByProcessId(models[0].ProcessId).ToModels();
                    FillCheckBoxLists(models[0].ProcessId);
                    ViewBag.ProcessId = models[0].ProcessId;
                    ViewBag.Message = "Success";
                    ViewBag.Style = "inline-block";
                    return PartialView("_AddActionTarget", actionTargetModels);
                }
                else
                {
                    ViewBag.Message = "There is Related Data";
                    ViewBag.Style = "inline-block";
                    return BackToActivityTargets(models[0].ProcessId.ToString());
                }
            }
            catch(Exception /*ex*/)
            {
                return BackToActivityTargets(models[0].ProcessId.ToString());
            }
        }

        [AjaxOnly]
        public ActionResult CreateStates(StateAddModel[] models)
        {

            Core.Process tempProcess = ProcessService.Obj.FindById(models[0].ProcessId);
            var isValid = true;
            #region Remove Deleted Items   
            List<State> processStates = tempProcess.States.ToList();
            foreach (State state in processStates)
            {
                bool isStateDeleted = true;
                foreach (StateAddModel stateAddModel in models)
                {
                    if (state.StateId == stateAddModel.StateId)
                    {
                        isStateDeleted = false;
                        if (stateAddModel.ActivityNames == null)
                        {
                            stateAddModel.ActivityNames = new Dictionary<string, string>();
                        }
                        List<Activity> stateActivities = state.Activities.ToList();
                        foreach (Activity stateActivity in stateActivities)
                        {

                            bool isActivityDeleted = true;
                            foreach (KeyValuePair<string, string> activity in stateAddModel.ActivityNames)
                            {
                                if (stateActivity.ActivityId == int.Parse(activity.Key))
                                {
                                    isActivityDeleted = false;
                                    break;
                                }
                            }
                            if (isActivityDeleted)
                                state.RemoveActivity(stateActivity);
                        }
                        break;
                    }
                }
                if (isStateDeleted)
                {
                    if ((from t in tempProcess.Transitions where t.CurrentStateId == state.StateId || t.NextStateId == state.StateId select t).ToList().Count == 0)
                    {
                        var tempState = StateService.Obj.FindById(state.StateId);
                        StateService.Obj.Delete(tempState);
                    }
                    else
                    {
                        isValid = false;
                    }
                }
            }
            #endregion
            tempProcess = ProcessService.Obj.FindById(models[0].ProcessId);
            try
            {
                #region Add and Update Groups and its members
                foreach (StateAddModel stateAddModel in models)
                {
                    var tempState = (from s in tempProcess.States where s.StateId == stateAddModel.StateId select s).FirstOrDefault();
                    if (tempState == null)
                    {
                        tempState = new State();
                    }
                    var stateTypeResult = StateTypeService.Obj.FindById(stateAddModel.StateTypeId);
                    tempState = tempState.Update(stateAddModel.Name, stateAddModel.Description, stateTypeResult);
                    //Group tempGroup = GroupService.Obj.FindById(groupAddModel.GroupId);
                    if (stateAddModel.ActivityNames == null)
                    {
                        stateAddModel.ActivityNames = new Dictionary<string, string>();
                    }
                    foreach (KeyValuePair<string, string> activity in stateAddModel.ActivityNames)
                    {
                        Activity tempActivity = (from s in tempProcess.Activities where s.ActivityId == int.Parse(activity.Key) select s).SingleOrDefault();
                        tempState.AddActivity(tempActivity);
                    }

                    tempProcess = tempProcess.AddState(tempState);

                    tempProcess = ProcessService.Obj.Update(tempProcess);
                }
                #endregion

                #region Prepare Transition View
                if (isValid)
                {
                    List<TransitionAddModel> transitionModels = tempProcess.Transitions.ToList().ToModels();
                    //ActivityAddModel activityModel = new ActivityAddModel();
                    //activityModel.ProcessId = models[0].ProcessId;
                    CreateTransitionData(models[0].ProcessId);
                    ViewBag.ProcessId = models[0].ProcessId;
                    ViewBag.Message = "Success";
                    ViewBag.Style = "inline-block";
                    return PartialView("_AddTransition", transitionModels);
                }
                else
                {
                    ViewBag.Message = "There is Related Data";
                    ViewBag.Style = "inline-block";
                    return BackToStates(models[0].ProcessId.ToString());
                }
                #endregion
            }
            catch (Exception /*ex*/)
            {
                return BackToStates(models[0].ProcessId.ToString());
            }
        }



        [AjaxOnly]
        public ActionResult CreateTransitions(TransitionAddModel[] models)
        {

            try
            {

                Core.Process tempProcess = ProcessService.Obj.FindById(models[0].ProcessId);
                #region Remove Deleted Items   
                List<Transition> processTransitions = tempProcess.Transitions.ToList();
                foreach (Transition transition in processTransitions)
                {
                    bool isTransitionDeleted = true;
                    foreach (TransitionAddModel transitionAddModel in models)
                    {
                        if (transition.TransitionId == transitionAddModel.TransitionId)
                        {
                            isTransitionDeleted = false;
                            if (transitionAddModel.ActivityNames == null)
                            {
                                transitionAddModel.ActivityNames = new Dictionary<string, string>();
                            }
                            List<Activity> transitionActivities = transition.Activities.ToList();
                            foreach (Activity transitionActivity in transitionActivities)
                            {

                                bool isActivityDeleted = true;
                                foreach (KeyValuePair<string, string> activity in transitionAddModel.ActivityNames)
                                {
                                    if (transitionActivity.ActivityId == int.Parse(activity.Key))
                                    {
                                        isActivityDeleted = false;
                                        break;
                                    }
                                }
                                if (isActivityDeleted)
                                    transition.RemoveActivity(transitionActivity);
                            }

                            if (transitionAddModel.ActionNames == null)
                            {
                                transitionAddModel.ActivityNames = new Dictionary<string, string>();
                            }
                            List<Core.Action> transitionActions = transition.Actions.ToList();
                            foreach (Core.Action transitionAction in transitionActions)
                            {

                                bool isActionDeleted = true;
                                foreach (KeyValuePair<string, string> action in transitionAddModel.ActionNames)
                                {
                                    if (transitionAction.ActionId == int.Parse(action.Key))
                                    {
                                        isActionDeleted = false;
                                        break;
                                    }
                                }
                                if (isActionDeleted)
                                    transition.RemoveAction(transitionAction);
                            }
                            break;
                        }
                    }
                    if (isTransitionDeleted)
                    {
                        var tempTransition = TransitionService.Obj.FindById(transition.TransitionId);
                        TransitionService.Obj.Delete(tempTransition);
                    }
                }
                #endregion
                tempProcess = ProcessService.Obj.FindById(models[0].ProcessId);
                foreach (TransitionAddModel transitionAddModel in models)
                {
                    var tempTransition = (from s in tempProcess.Transitions where s.TransitionId == transitionAddModel.TransitionId select s).FirstOrDefault();
                    if (tempTransition == null)
                        tempTransition = new Transition();

                    State currentState = (from s in tempProcess.States where s.StateId == transitionAddModel.CurrentStateId select s).SingleOrDefault();
                    State nextState = (from s in tempProcess.States where s.StateId == transitionAddModel.NextStateId select s).SingleOrDefault();

                    tempTransition = tempTransition.Update(currentState, nextState);

                    if (transitionAddModel.ActivityNames == null)
                        transitionAddModel.ActivityNames = new Dictionary<string, string>();

                    foreach (KeyValuePair<string, string> activity in transitionAddModel.ActivityNames)
                    {
                        Activity tempActivity = (from s in tempProcess.Activities where s.ActivityId == int.Parse(activity.Key) select s).SingleOrDefault();
                        tempTransition.AddActivity(tempActivity);
                    }

                    if (transitionAddModel.ActionNames == null)
                        transitionAddModel.ActionNames = new Dictionary<string, string>();

                    foreach (KeyValuePair<string, string> action in transitionAddModel.ActionNames)
                    {
                        Core.Action tempAction = (from s in tempProcess.Actions where s.ActionId == int.Parse(action.Key) select s).SingleOrDefault();
                        tempTransition.AddAction(tempAction);
                    }

                    tempProcess = tempProcess.AddTransition(tempTransition);

                    tempProcess = ProcessService.Obj.Update(tempProcess);
                }

                ViewBag.ProcessId = models[0].ProcessId;
                ViewBag.Message = "Success";
                ViewBag.Style = "inline-block";

                TransitionAddModel transitionModel = new TransitionAddModel();
                transitionModel.ProcessId = models[0].ProcessId;

                transitionModel = CreateTransitionStatesList(transitionModel);
                transitionModel = CreateTransitionActionsList(transitionModel);
                transitionModel = CreateTransitionActivitiesList(transitionModel);
                FillDropDownLists();

                return RedirectToAction("Index");
            }
            catch (ValidationException)
            {
                //valEx.PopulateIn(this.ModelAction);
                return View(models[0]);
            }
            catch (BusinessRuleException/* buzRuleEx*/)
            {
                //AddMCIMessage(buzRuleEx.Message, MCIMessageType.Danger, 10);
                return View(models[0]);
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString(), repEx.StackTrace);
                //AddMCIMessage(repEx.Message, MCIMessageType.Danger, 10);
                return View(models[0]);
            }
        }



        [AjaxOnly]
        public ActionResult CreateGroups(GroupAddModel[] models)
        {

            try
            {
                Core.Process tempProcess = ProcessService.Obj.FindById(models[0].ProcessId);
                var isValid = true;
                #region Remove Deleted Items   
                List<Group> processGroups = tempProcess.Groups.ToList();
                foreach (Group group in processGroups)
                {
                    bool isGroupDeleted = true;
                    foreach (GroupAddModel groupAddModel in models)
                    {
                        if (group.GroupId == groupAddModel.GroupId)
                        {
                            isGroupDeleted = false;
                            if (groupAddModel.UserNames == null)
                            {
                                groupAddModel.UserNames = new Dictionary<string, string>();
                            }
                            List<User> groupUsers = group.Users.ToList();
                            foreach (User member in groupUsers)
                            {

                                bool isMemberDeleted = true;
                                foreach (KeyValuePair<string, string> user in groupAddModel.UserNames)
                                {
                                    if (member.Id == int.Parse(user.Key))
                                    {
                                        isMemberDeleted = false;
                                        break;
                                    }
                                }
                                if (isMemberDeleted)
                                    group.RemoveMember(member);
                            }
                            break;
                        }
                    }
                    if (isGroupDeleted)
                    {
                        bool hasActivityTarget = ActivityTargetService.Obj.FindByGroupId(group.GroupId).TotalCount > 0 ? true : false;
                        bool hasActionTarget = ActionTargetService.Obj.FindByGroupId(group.GroupId).TotalCount > 0 ? true : false;
                        if (!(hasActivityTarget || hasActionTarget))
                        {
                            var tempGroup = GroupService.Obj.FindById(group.GroupId);
                            GroupService.Obj.Delete(tempGroup);
                        }
                        else
                        {
                            isValid = false;
                        }
                    }
                }
                #endregion
                tempProcess = ProcessService.Obj.FindById(models[0].ProcessId);
                #region Add and Update Groups and its members
                foreach (GroupAddModel groupAddModel in models)
                {
                    var tempGroup = (from g in tempProcess.Groups where g.GroupId == groupAddModel.GroupId select g).FirstOrDefault();
                    if (tempGroup == null)
                    {
                        tempGroup = new Group();
                    }
                    tempGroup = tempGroup.UpdateName(groupAddModel.Name);
                    if (groupAddModel.UserNames == null)
                    {
                        groupAddModel.UserNames = new Dictionary<string, string>();
                    }
                    foreach (KeyValuePair<string, string> user in groupAddModel.UserNames)
                    {
                        User tempUser = UserService.Obj.FindById(int.Parse(user.Key));
                        tempGroup.AddMember(tempUser);
                    }
                    tempProcess = tempProcess.AddGroup(tempGroup);

                    tempProcess = ProcessService.Obj.Update(tempProcess);
                }
                #endregion


                if (isValid)
                {
                    List<ActivityAddModel> activityModels = tempProcess.Activities.ToList().ToModels();
                    FillDropDownLists();
                    ViewBag.ProcessId = models[0].ProcessId;
                    ViewBag.Message = "Success";
                    ViewBag.Style = "inline-block";
                    AddMCIMessage("Success", MCIMessageType.Info);
                    return PartialView("_AddActivity", activityModels);

                }
                else
                {
                    ViewBag.Message = "There is Related Data";
                    ViewBag.Style = "inline-block";
                    return BackToGroups(models[0].ProcessId.ToString());
                }
            }

            #region Handle Exceptions
            catch (Exception /*ex*/)
            {
                return BackToGroups(models[0].ProcessId.ToString());
            }
            #endregion
        }
        [AjaxOnly]
        public ActionResult CreateActionTargets(ActionTargetAddModel[] models)
        {
            try
            {
                int processId = models[0].ProcessId;
                Core.Process tempProcess = ProcessService.Obj.FindById(processId);
                #region Remove Deleted Items   
                List<ActionTarget> actionTargets = ActionTargetService.Obj.GetActionTargetesByProcessId(processId).Items.ToList();
                foreach (ActionTarget actionTarget in actionTargets)
                {
                    bool isActionTargetDeleted = true;
                    foreach (ActionTargetAddModel actionTargetAddModel in models)
                    {
                        if (actionTarget.ActionTargetId == actionTargetAddModel.ActionTargetId)
                        {
                            isActionTargetDeleted = false;
                            break;
                        }
                    }
                    if (isActionTargetDeleted)
                    {
                        ActionTarget tempActionTarget = ActionTargetService.Obj.FindById(actionTarget.ActionTargetId);
                        ActionTargetService.Obj.Delete(tempActionTarget);
                    }
                }
                #endregion

                foreach (ActionTargetAddModel model in models)
                {
                    Core.Action tempAction = ActionService.Obj.FindById(model.ActionId);
                    Target tempTarget = TargetService.Obj.FindById(model.TargetId);
                    Group tempGroup = GroupService.Obj.FindById(model.GroupId);
                    ActionTarget tempActionTarget = new ActionTarget();
                    if (model.ActionTargetId != 0)
                        tempActionTarget = ActionTargetService.Obj.FindById(model.ActionTargetId);

                    tempActionTarget.Update(tempAction, tempTarget, tempGroup);

                    ActionTargetService.Obj.Update(tempActionTarget);
                }
                #region Prepare Activity View

                List<StateAddModel> stateModels = tempProcess.States.ToList().ToModels();
                FillCheckBoxLists(tempProcess.ProcessId);
                FillDropDownLists();
                ViewBag.ProcessId = tempProcess.ProcessId;
                ViewBag.Message = "Success";
                ViewBag.Style = "inline-block";
                return PartialView("_AddState", stateModels);

                #endregion                

            }
            catch (ValidationException)
            {
                //valEx.PopulateIn(this.ModelAction);
                return View(models[0]);
            }
            catch (BusinessRuleException /*buzRuleEx*/)
            {
                //AddMCIMessage(buzRuleEx.Message, MCIMessageType.Danger, 10);
                return View(models[0]);
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString(), repEx.StackTrace);
                //AddMCIMessage(repEx.Message, MCIMessageType.Danger, 10);
                return View(models[0]);
            }
        }

        [AjaxOnly]
        public ActionResult CreateActivityTargets(ActivityTargetAddModel[] models)
        {
            try
            {
                int processId = models[0].ProcessId;
                Core.Process tempProcess = ProcessService.Obj.FindById(processId);
                #region Remove Deleted Items   
                List<ActivityTarget> activityTargets = ActivityTargetService.Obj.GetActivityTargetesByProcessId(processId).Items.ToList();
                foreach (ActivityTarget activityTarget in activityTargets)
                {
                    bool isActivityTargetDeleted = true;
                    foreach (ActivityTargetAddModel activityTargetAddModel in models)
                    {
                        if (activityTarget.ActivityTargetId == activityTargetAddModel.ActivityTargetId)
                        {
                            isActivityTargetDeleted = false;
                            break;
                        }
                    }
                    if (isActivityTargetDeleted)
                    {
                        ActivityTarget tempActivityTarget = ActivityTargetService.Obj.FindById(activityTarget.ActivityTargetId);
                        ActivityTargetService.Obj.Delete(tempActivityTarget);
                    }
                }
                #endregion

                foreach (ActivityTargetAddModel model in models)
                {
                    Activity tempActivity = ActivityService.Obj.FindById(model.ActivityId);
                    Target tempTarget = TargetService.Obj.FindById(model.TargetId);
                    Group tempGroup = GroupService.Obj.FindById(model.GroupId);
                    ActivityTarget tempActivityTarget = new ActivityTarget();
                    if (model.ActivityTargetId != 0)
                        tempActivityTarget = ActivityTargetService.Obj.FindById(model.ActivityTargetId);

                    tempActivityTarget.Update(tempActivity, tempTarget, tempGroup);

                    ActivityTargetService.Obj.Update(tempActivityTarget);
                }
                #region Prepare Activity View
                List<ActionAddModel> actionModels = tempProcess.Actions.ToList().ToModels();
                FillDropDownLists();
                ViewBag.ProcessId = models[0].ProcessId;
                ViewBag.Message = "Success";
                ViewBag.Style = "inline-block";
                return PartialView("_AddAction", actionModels);
                #endregion
            }
            catch (ValidationException)
            {
                //valEx.PopulateIn(this.ModelAction);
                return View(models[0]);
            }
            catch (BusinessRuleException /*buzRuleEx*/)
            {
                //AddMCIMessage(buzRuleEx.Message, MCIMessageType.Danger, 10);
                return View(models[0]);
            }
            catch (RepositoryException repEx)
            {
                Trace.TraceError("A Repository Error has occurred as the followings: {0}", repEx.ToString(), repEx.StackTrace);
                //AddMCIMessage(repEx.Message, MCIMessageType.Danger, 10);
                return View(models[0]);
            }
        }

        #region Back
        [AjaxOnly]
        public ActionResult BackToProcess(string processId)
        {
            Core.Process tempProcess = ProcessService.Obj.FindById(int.Parse(processId));
            var processModel = tempProcess.ToModel();
            CreateUsersList();
            ViewBag.ProcessId = processModel.ProcessId;
            if (ViewBag.Style == null)
                ViewBag.Style = "none";
            return PartialView("_AddProcess", processModel);
        }
        [AjaxOnly]
        public ActionResult BackToGroups(string processId)
        {
            Core.Process tempProcess = ProcessService.Obj.FindById(int.Parse(processId));

            List<GroupAddModel> groupModels = tempProcess.Groups.ToList().ToModels();
            CreateUsersList();
            ViewBag.ProcessId = tempProcess.ProcessId;
            if (ViewBag.Style == null)
                ViewBag.Style = "none";
            return PartialView("_AddGroup", groupModels);
        }
        [AjaxOnly]
        public ActionResult BackToActivities(string processId)
        {
            Core.Process tempProcess = ProcessService.Obj.FindById(int.Parse(processId));

            List<ActivityAddModel> activityModels = tempProcess.Activities.ToList().ToModels();
            FillDropDownLists();
            ViewBag.ProcessId = tempProcess.ProcessId;
            if (ViewBag.Style == null)
                ViewBag.Style = "none";
            return PartialView("_AddActivity", activityModels);
        }

        [AjaxOnly]
        public ActionResult BackToActivityTargets(string processId)
        {
            List<ActivityTargetAddModel> activityTargetModels = ActivityTargetService.Obj.GetActivityTargetesByProcessId(int.Parse(processId)).ToModels();
            FillCheckBoxLists(int.Parse(processId));
            ViewBag.ProcessId = int.Parse(processId);

            if (ViewBag.Style == null)
                ViewBag.Style = "none";
            return PartialView("_AddActivityTarget", activityTargetModels);
        }
        [AjaxOnly]
        public ActionResult BackToActions(string processId)
        {
            Core.Process tempProcess = ProcessService.Obj.FindById(int.Parse(processId));

            List<ActionAddModel> actionModels = tempProcess.Actions.ToList().ToModels();
            FillDropDownLists();
            ViewBag.ProcessId = tempProcess.ProcessId;
            if (ViewBag.Style == null)
                ViewBag.Style = "none";
            return PartialView("_AddAction", actionModels);
        }
        [AjaxOnly]
        public ActionResult BackToActionTargets(string processId)
        {
            List<ActionTargetAddModel> actionTargetModels = ActionTargetService.Obj.GetActionTargetesByProcessId(int.Parse(processId)).ToModels();
            FillCheckBoxLists(int.Parse(processId));
            ViewBag.ProcessId = int.Parse(processId);
            if (ViewBag.Style == null)
                ViewBag.Style = "none";
            return PartialView("_AddActionTarget", actionTargetModels);
        }
        [AjaxOnly]
        public ActionResult BackToStates(string processId)
        {
            Core.Process tempProcess = ProcessService.Obj.FindById(int.Parse(processId));

            List<StateAddModel> stateModels = tempProcess.States.ToList().ToModels();
            FillCheckBoxLists(tempProcess.ProcessId);
            FillDropDownLists();
            ViewBag.ProcessId = tempProcess.ProcessId;
            if (ViewBag.Style == null)
                ViewBag.Style = "none";
            return PartialView("_AddState", stateModels);
        }
        [AjaxOnly]
        public ActionResult BackToTransitions(string processId)
        {
            Core.Process tempProcess = ProcessService.Obj.FindById(int.Parse(processId));

            List<TransitionAddModel> transitionModels = tempProcess.Transitions.ToList().ToModels();
            CreateTransitionData(int.Parse(processId));
            ViewBag.ProcessId = int.Parse(processId);
            return PartialView("_AddTransition", transitionModels);
        }
        #endregion
        #region
        private void CreateUsersList()
        {
            var users = UserService.Obj.GetAllUsersDictionary();
            ViewBag.Users = users;
        }
        private TransitionAddModel CreateTransitionStatesList(TransitionAddModel transition)
        {
            return transition.FillDDLsWithStates();
        }
        private TransitionAddModel CreateTransitionActivitiesList(TransitionAddModel transition)
        {
            return transition.FillDDLsWithActivities();
        }

        private TransitionAddModel CreateTransitionActionsList(TransitionAddModel transition)
        {
            return transition.FillDDLsWithActions();
        }
        public void FillDropDownLists()
        {
            var activityTypes = ActivityTypeService.Obj.GetAllActivityTypes().Items.ToList().OrderBy(x => x.ActivityTypeId);
            ViewBag.activityTypes = new SelectList(activityTypes, "ActivityTypeId", "Name");

            var actionTypes = ActionTypeService.Obj.GetAllActionTypes().Items.ToList().OrderBy(x => x.ActionTypeId);
            ViewBag.ActionTypes = new SelectList(actionTypes, "ActionTypeId", "Name");

            var stateTypes = StateTypeService.Obj.GetAllStateTypes().Items.ToList().OrderBy(x => x.StateTypeId);
            ViewBag.StateTypes = new SelectList(stateTypes, "StateTypeId", "Name");
        }
        public void FillCheckBoxLists(int processId)
        {
            var actions = ActionService.Obj.FindByProcessId(processId).Items.ToList().OrderBy(x => x.ActionId);
            ViewBag.Actions = new SelectList(actions, "ActionId", "Name");

            var activities = ActivityService.Obj.FindByProcessId(processId).Items.ToList().OrderBy(x => x.ActivityId);
            ViewBag.Activities = new SelectList(activities, "ActivityId", "Name");

            var targets = TargetService.Obj.GetAllTargetes().Items.ToList().OrderBy(x => x.TargetId);
            ViewBag.Targets = new SelectList(targets, "TargetId", "Name");

            var groups = GroupService.Obj.FindByProcessId(processId).Items.ToList().OrderBy(x => x.GroupId);
            ViewBag.groups = new SelectList(groups, "GroupId", "Name");
        }

        private void CreateTransitionData(int processId)
        {
            ViewBag.StateNames = StateService.Obj.GetAllStatesDictionary(processId);
            ViewBag.ActionNames = ActionService.Obj.GetAllActionsDictionary(processId);
            ViewBag.ActivityNames = ActivityService.Obj.GetAllActivitiesDictionary(processId);
        }
        #endregion
    }
}