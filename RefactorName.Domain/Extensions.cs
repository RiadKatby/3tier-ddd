using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RefactorName.Core;
using RefactorName.RepositoryInterface;
using RefactorName.RepositoryInterface.Queries;

namespace RefactorName.Domain
{
    public static class Extensions
    {
        public static bool IsUserNameUnique(this User user, bool isUpdatedCheck)
        {
            var queryRepository = RepositoryFactory.CreateQueryRepository();
            var constraints = new QueryConstraints<User>()
                                 .Where(x => x.UserName == user.UserName);


            if (isUpdatedCheck)
                constraints = constraints.AndAlso(x => x.Id != user.Id);

            return queryRepository.SingleOrDefault(constraints) == null;
        }

        public static bool IsValidADUser(this User user)
        {
            IActiveDirectoryRepository activeDirectoryRepository = RepositoryFactory.CreateWebSvc<IActiveDirectoryRepository>("ActiveDirectoryRepository");

            var adUser = activeDirectoryRepository.GetUserInfo(user.UserName);

            return adUser != null;
        }
    }
}
