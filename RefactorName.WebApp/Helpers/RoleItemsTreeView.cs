using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp.Helpers
{
    public class RoleItemsTreeView
    {
        public List<RolesTreeViewModel> Roles { get; set; }

        public RoleItemsTreeView()
        {
            this.Roles = new List<RolesTreeViewModel>();
        }
    }
}