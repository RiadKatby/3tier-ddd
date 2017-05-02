using System.Linq;
using RefactorName.Core;
using RefactorName.Domain;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Collections;

namespace RefactorName.WebApp
{
    public class UserIndexModel
    {

        public UserSearchCriteriaModel userSearchCriteriaModel { get; set; }

        public WebGridList<UserModel> Items { get; set; }

        public Dictionary<string, string> RoleNames { get; set; }

        public UserIndexModel()
        {
            RoleNames = new Dictionary<string, string>();
            userSearchCriteriaModel = new UserSearchCriteriaModel();
            Activation = new Dictionary<string, string>();
            ActiveDirectory = new Dictionary<string, string>();
        }

        public UserIndexModel FillDDLs()
        {
            RoleNames = Core.RoleNames.GetRolesWithCaptions();

            Activation.Add("", "All");
            Activation.Add("True", "Active");
            Activation.Add("False", "Not Active");

            ActiveDirectory.Add("", "All");
            ActiveDirectory.Add("True", "Yes");
            ActiveDirectory.Add("False", "No");

            return this;
        }

        public UserIndexModel FillDDLsWithoutUserPermission()
        {
            //RoleNames = Core.RoleNames.GetRolesWithCaptionsWithoutUserPermission();

            Activation.Add("", "All");
            Activation.Add("True", "Active");
            Activation.Add("False", "Not Active");

            ActiveDirectory.Add("", "All");
            ActiveDirectory.Add("True", "Yes");
            ActiveDirectory.Add("False", "No");

            return this;
        }

        public UserIndexModel FillDDLsWithUserPermissionOnly()
        {
            //RoleNames = Core.RoleNames.GetRolesWithCaptionsWithUserPermissionOnly();

            Activation.Add("", "All");
            Activation.Add("True", "Active");
            Activation.Add("False", "Not Active");

            ActiveDirectory.Add("", "All");
            ActiveDirectory.Add("True", "Yes");
            ActiveDirectory.Add("False", "No");

            return this;
        }

        public Dictionary<string, string> Activation { get; set; }
        public Dictionary<string, string> ActiveDirectory { get; set; }

    }
}