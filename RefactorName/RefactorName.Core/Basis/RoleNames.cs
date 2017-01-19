using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RefactorName.Core
{
    public static class RoleNames
    {
        /// <summary>
        /// This user can access the complete system privileges.
        /// </summary>
        [PermissionCaption("مدير كامل النظام")]
        public const string SuperAdministrator = "/";

        [PermissionCaption("إدارة المستخدمين")]
        public const string Users = "/Users";

        [PermissionCaption("عرض المستخدمين")]
        public const string UsersView = "/Users/View";

        [PermissionCaption("إضافة مستخدم")]
        public const string UsersAdd = "/Users/Add";

        [PermissionCaption("تعديل مستخدم")]
        public const string UsersEdit = "/Users/Edit";


        /// <summary>
        /// This user can access the some of the system privileges in his/her organization.
        /// </summary>

        public static Dictionary<string, string> GetRolesWithCaptions()
        {
            var result = new Dictionary<string, string>();

            foreach (var field in typeof(RoleNames).GetFields())
            {
                if (field.IsPublic && field.IsLiteral)
                {
                    string name = field.GetValue(null) as string;
                    string caption = name;

                    object[] attrs = field.GetCustomAttributes(typeof(PermissionCaptionAttribute), false);
                    if (attrs != null && attrs.Length > 0)
                        caption = ((PermissionCaptionAttribute)attrs[0]).Caption;

                    result.Add(name, caption);
                }
            }

            return result;
        }

        public static IEnumerable<string> GetAllParentRoles(string childRole)
        {
            foreach (var role in RoleNames.GetRolesWithCaptions())
            {
                if (childRole.StartsWith(role.Key.ToString()))
                    yield return role.Key.ToString();
            }
        }

    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public sealed class PermissionCaptionAttribute : Attribute
    {
        // This is a positional argument
        public PermissionCaptionAttribute(string caption)
        {
            this.Caption = caption;
        }

        public string Caption { get; private set; }
    }
}
