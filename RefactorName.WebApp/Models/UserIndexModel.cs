using System.Linq;
using RefactorName.Core;
using RefactorName.Domain;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Collections;

namespace RefactorName.WebApp.Models
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
        }

        public UserIndexModel FillDDLs()
        {
            RoleNames = Core.RoleNames.GetRolesWithCaptions();

            Activation.Add("", "الكل");
            Activation.Add("True", "مفعل");
            Activation.Add("False", "غير مفعل");
            return this;
        }

        public Dictionary<string, string> Activation { get; set; }
    }
}