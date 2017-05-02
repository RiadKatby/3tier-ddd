using Microsoft.AspNet.Identity;
using RefactorName.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.RepositoryInterface
{
    public interface IIdentityUserRepository : IUserStore<User, int>,
        IUserPasswordStore<User, int>,
        IUserEmailStore<User, int>,
        IUserLockoutStore<User, int>,
        IUserTwoFactorStore<User, int>,
        IUserRoleStore<User, int>
    {
    }
}
