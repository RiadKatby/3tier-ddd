using Microsoft.AspNet.Identity;
using RefactorName.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using RefactorName.Domain;

namespace RefactorName.WebApp.Infrastructure.Security
{
    public class AppUserStore : IUserStore<User, int>, IUserPasswordStore<User, int>, IUserLoginStore<User, int>, IUserSecurityStampStore<User, int>, IUserLockoutStore<User, int>, IUserEmailStore<User,int>
    {
        public static List<User> users = new List<User>
        {
            new User { Id = 1, UserName = "morikapt@gmail.com", Email = "morikapt@gmail.com" },
            new User { Id = 2, UserName = "morikapt@hotmail.com", Email = "morikapt@hotmail.com" }
        };

        #region IUserStore<User, int> Interface Members
        public Task CreateAsync(User user)
        {
            return Task.Factory.StartNew(() => { });
        }

        public Task DeleteAsync(User user)
        {
            return Task.Factory.StartNew(() => { });
        }

        public void Dispose()
        {

        }

        public Task<User> FindByIdAsync(int userId)
        {
            return Task.Factory.StartNew<User>(() => { return users.FirstOrDefault(x => x.Id == userId); });
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return Task.Factory.StartNew<User>(() => { return users.FirstOrDefault(x => x.UserName.Equals(userName)); });
        }

        public Task UpdateAsync(User user)
        {
            return Task.Factory.StartNew(() => { });
        }

        #endregion

        #region IUserPasswordStore<User, int> Intercace Members 
        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.Factory.StartNew<string>(() => "adsf");
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.Factory.StartNew<bool>(() => true);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            return Task.Factory.StartNew(() => { });
        }

        #endregion

        #region IUserLoginStore<User, int> Interface Members 

        public Task AddLoginAsync(User user, UserLoginInfo login)
        {
            return Task.Factory.StartNew(() => { });
        }

        public Task<User> FindAsync(UserLoginInfo login)
        {
            return Task.Factory.StartNew<User>(() => { return new User(); });
        }
        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            return Task.Factory.StartNew<IList<UserLoginInfo>>(() => { return new List<UserLoginInfo>(); });
        }
        public Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            return Task.Factory.StartNew(() => { });
        }

        #endregion

        #region IUserSecurityStampStore<User, int> Interface Members 

        public Task<string> GetSecurityStampAsync(User user)
        {
            return Task.Factory.StartNew<string>(() => { return "asdf"; });
        }
        public Task SetSecurityStampAsync(User user, string stamp)
        {
            return Task.Factory.StartNew(() => { });
        }

        #endregion

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.Factory.StartNew<int>(() => 1);
        }
        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.Factory.StartNew<bool>(() => true);
        }
        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            return Task.Factory.StartNew<DateTimeOffset>(() => new DateTimeOffset());
        }
        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            return Task.Factory.StartNew<int>(() => 1);
        }
        public Task ResetAccessFailedCountAsync(User user)
        {
            return Task.Factory.StartNew(() => { });
        }
        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            return Task.Factory.StartNew(() => { });
        }
        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            return Task.Factory.StartNew(() => { });
        }

        public Task SetEmailAsync(User user, string email)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(User user)
        {
            return Task.FromResult<string>(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByEmailAsync(string email)
        {
            return Task.FromResult<User>(UserService.Obj.FindByEmail(email));
        }

        public Task<User> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}