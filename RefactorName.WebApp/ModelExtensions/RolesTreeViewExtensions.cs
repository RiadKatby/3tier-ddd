using RefactorName.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RefactorName.Web.ModelExtensions
{
    public static class RolesTreeViewExtensions
    {
        public static RolesTreeViewModel ToModel(this RolesTreeViewModel entity, IEnumerable<SelectListItem> selectList, UserEditModel model)
        {
            RolesTreeViewModel root = selectList.Where(c => c.Value == "/")
                                                .Select(r => new RolesTreeViewModel()
                                                {
                                                    id = r.Value,
                                                    name = r.Text,
                                                    selected = (model.Roles != null) && model.Roles.Any(sr => sr == r.Value)
                                                }).FirstOrDefault();

            foreach (KeyValuePair<string, string> kvp in selectList.Select(c => new KeyValuePair<string, string>(c.Text, c.Value)))
            {
                if (kvp.Value == "/" || root == null)
                    continue;

                RolesTreeViewModel parent = root;

                bool selected = (model.Roles != null) && model.Roles.Any(c => c == kvp.Value);

                if (!string.IsNullOrEmpty(kvp.Value))
                {
                    RolesTreeViewModel child = null;
                    foreach (string part in kvp.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string name = part.Trim();
                        child = parent.children.Find(n => n.root == name);
                        if (child == null)
                        {
                            child = new RolesTreeViewModel { root = name, name = kvp.Key, id = kvp.Value, selected = selected };

                            parent.children.Add(child);
                        }
                        parent = child;
                    }
                }
            }

            return root;
        }

    }
}