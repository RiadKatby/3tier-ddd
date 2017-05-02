using RefactorName.Core;
using RefactorName.WebApp.Areas.Backend.Models;
using RefactorName.WebApp.Models;
using System.Collections.Generic;
using System.Linq;
using static RefactorName.Core.Enum;

namespace RefactorName.WebApp
{
    public static class UserExtensions
    {
        public static UserModel ToModel(this User entity)
        {

            UserModel model = new UserModel();
            model.UserID = entity.Id;
            //model.UserName = entity.UserName;
            model.PhoneNumber = entity.PhoneNumber;
            model.FullName = entity.FullName;
            model.Email = entity.Email;
            model.IsActive = entity.IsActive;
            model.UserStatus = Status.Exist;
            model.Roles = entity.Roles.Select(r => r.Name).ToArray();
            return model;
            //return new UserModel
            //{
            //    UserID = entity.Id,
            //    UserName = entity.UserName,
            //    Mobile = entity.Mobile,
            //    FullName = entity.FullName,
            //    Email = entity.Email,
            //    IsActive = entity.IsActive,
            //    IsADUser = entity.IsADUser,
            //    Roles = entity.Roles.Select(r => r.Name).ToArray()
            //};
        }

        public static List<UserModel> ToModels(this IQueryResult<User> users)
        {
            var result = from c in users.Items select c.ToModel();
            return result.ToList();
        }

        public static WebGridList<UserModel> ToWebGridListModel(this IQueryResult<User> users)
        {
            var result = new WebGridList<UserModel>
            {
                List = users.ToModels(),
                PageSize = users.PageSize,
                RowCount = users.TotalCount,
                PageIndex = users.PageNumber
            };

            return result;
        }

        public static UserEditModel ToEditModel(this UserModel model)
        {
            return new UserEditModel(model).FillDDLs();
        }
    }
}