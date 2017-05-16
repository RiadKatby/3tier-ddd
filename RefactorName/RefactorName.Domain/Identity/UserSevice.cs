using RefactorName.Core;
using RefactorName.RepositoryInterface;
using RefactorName.RepositoryInterface.Queries;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorName.Domain
{
    public class UserService : IUserStore<User, int>,
        IUserPasswordStore<User, int>,
        IUserEmailStore<User, int>,
        IUserLockoutStore<User, int>,
        IUserTwoFactorStore<User, int>,
        IUserRoleStore<User, int>
    {
        public static UserService Obj { get; private set; }

        private static IGenericQueryRepository queryRepository;
        private static IGenericRepository repository;

        static UserService()
        {
            Obj = new UserService();
        }

        private UserService()
        {
            queryRepository = RepositoryFactory.CreateQueryRepository();
            repository = RepositoryFactory.CreateRepository();
        }

        public IQueryResult<User> Find(UserSearchCriteria userSearchCriteria)
        {
            if (userSearchCriteria == null)
                throw new ArgumentNullException("userSearchCriteria", "must not be null.");

            var constraints = new QueryConstraints<User>()
                .IncludePath(u => u.Roles)
                .Page(userSearchCriteria.PageNumber, userSearchCriteria.PageSize)
                .Where(c => true);

            if (userSearchCriteria.IsActive.HasValue)
                constraints.AndAlso(c => c.IsActive == userSearchCriteria.IsActive.Value);

            if (!string.IsNullOrEmpty(userSearchCriteria.FullName))
                constraints.AndAlso(c => c.FullName.Contains(userSearchCriteria.FullName));

            if (!string.IsNullOrEmpty(userSearchCriteria.Mobile))
                constraints.AndAlso(c => c.Mobile.Contains(userSearchCriteria.Mobile));

            if (!string.IsNullOrEmpty(userSearchCriteria.RoleName))
                constraints.AndAlso(c => c.Roles.Any(r => r.Name == userSearchCriteria.RoleName));

            if (!string.IsNullOrEmpty(userSearchCriteria.UserName))
                constraints.AndAlso(c => c.UserName.Contains(userSearchCriteria.UserName));

            if (!string.IsNullOrEmpty(userSearchCriteria.Email))
                constraints.AndAlso(c => c.Email.Contains(userSearchCriteria.Email));

            if (string.IsNullOrEmpty(userSearchCriteria.Sort))
                constraints.SortByDescending(c => c.CreatedAt);
            //else if (userSearchCriteria.SortDirection == WebGridSortOrder.Ascending)
            //    constraints.SortBy(userSearchCriteria.Sort);
            else
                constraints.SortByDescending(userSearchCriteria.Sort);


            return queryRepository.Find(constraints);
        }

        public List<User> Find()
        {
            var constraints = new QueryConstraints<User>()
                .Page(1, int.MaxValue)
                .Where(u => u.PasswordHash == null);

            return queryRepository.Find(constraints).Items.ToList();
        }

        public User getByUsername(string username)
        {
            var constraints = new QueryConstraints<User>()
                .Page(1, int.MaxValue)
                .Where(u => u.IsActive == true)
                .SortBy("FullName")
                .AndAlso(u => u.UserName == username);

            return queryRepository.Find(constraints).Items.FirstOrDefault();
        }

        #region IUserStore
        public Task CreateAsync(User user)
        {
            return Task.FromResult(Create(user));
        }

        public Task DeleteAsync(User user)
        {
            return Task.FromResult(Delete(user));
        }

        public Task<User> FindByIdAsync(int userId)
        {
            return Task.FromResult(FindById(userId));
        }

        public Task<User> FindByNameAsync(string userName)
        {
            var result = FindByName(userName);
            return Task.FromResult(result);
        }

        public Task UpdateAsync(User user)
        {
            return Task.FromResult(Update(user));
        }

        public void Dispose()
        {
        }

        #endregion

        #region IUserStore (Core)

        public User Create(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null.");

            //if (!Thread.CurrentPrincipal.IsInRole(RoleNames.SuperAdministrator))
            //throw new PermissionException("You have no permission to execute this operation.", RoleNames.SuperAdministrator);

            if (!user.Validate())
                throw new ValidationException(typeof(User).Name, user.ValidationResults);

            if (!user.IsUserNameUnique(true))
                throw new BusinessRuleException("UserName must be unique", typeof(User).Name);

            return repository.Create<User>(user);
        }

        public bool Delete(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null.");

            //if (!Thread.CurrentPrincipal.IsInRole(RoleNames.ManageUsers))
            //    throw new PermissionException("You have no permission to execute this operation.", RoleNames.ManageUsers);

            return repository.Delete<User>(user);
        }

        public User FindById(int userId)
        {
            if (userId == 0)
                throw new ArgumentNullException("userID", "must not be null.");

            var constraints = new QueryConstraints<User>()
                .IncludePath(u => u.Roles)
                .Where(x => x.Id == userId);

            return queryRepository.SingleOrDefault(constraints);
        }

        public User FindByName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName", "must not be null or empty.");

            var constraints = new QueryConstraints<User>()
                .IncludePath(u => u.Roles)
                .Where(x => x.UserName == userName);

            return queryRepository.SingleOrDefault(constraints);
        }

        public User Update(User user)
        {
            if (user == null)
                throw new ArgumentNullException("entity", "must not be null.");

            //if (!Thread.CurrentPrincipal.IsInRole(RoleNames.ManageUsers))
            //    throw new PermissionException("You have no permission to execute current operation.", RoleNames.ManageUsers);

            if (!user.Validate())
                throw new ValidationException("Some data are not valid", user.ValidationResults);

            return repository.Update<User>(user);
        }

        #endregion

        #region IUserPasswordStore
        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(GetPasswordHash(user));
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(HasPassword(user));
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            SetPasswordHash(user, passwordHash);
            return Task.FromResult(0);
        }


        public Task<User> FindByIdAsync(string userId)
        {
            return FindByIdAsync(int.Parse(userId));
        }

        #endregion

        #region IUserPasswordStore (Core)
        public string GetPasswordHash(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null.");

            return user.PasswordHash;
        }

        public bool HasPassword(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null.");

            return !string.IsNullOrEmpty(user.PasswordHash);
        }

        public void SetPasswordHash(User user, string passwordHash)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null.");

            user.PasswordHash = passwordHash;
        }
        #endregion

        #region IUserEmailStore
        public Task<User> FindByEmailAsync(string email)
        {
            return Task.FromResult(FindByEmail(email));
        }

        public Task<string> GetEmailAsync(User user)
        {
            return Task.FromResult(GetEmail(user));
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            return Task.FromResult(GetEmailConfirmed(user));
        }

        public Task SetEmailAsync(User user, string email)
        {
            SetEmail(user, email);
            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            SetEmailConfirmed(user, confirmed);
            return Task.FromResult(0);
        }

        #endregion

        #region IUserEmailStore Core
        public User FindByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("email", "must not be null or empty.");

            var constraints = new QueryConstraints<User>()
                .Where(x => x.Email == email);

            return queryRepository.SingleOrDefault(constraints);
        }

        public string GetEmail(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null or empty.");

            return user.Email;
        }

        public bool GetEmailConfirmed(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null or empty.");

            return user.EmailConfirmed;
        }

        public void SetEmail(User user, string email)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null or empty.");

            user.Email = email;
        }

        public void SetEmailConfirmed(User user, bool confirmed)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null or empty.");

            user.EmailConfirmed = confirmed;
        }

        #endregion

        #region IUserLockoutStore
        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(GetAccessFailedCount(user));
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(GetLockoutEnabled(user));
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            return Task.FromResult(GetLockoutEndDate(user));
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            return Task.FromResult(IncrementAccessFailedCount(user));
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            ResetAccessFailedCount(user);
            return Task.FromResult(0);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            SetLockoutEnabled(user, enabled);
            return Task.FromResult(0);
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            SetLockoutEndDate(user, lockoutEnd);
            return Task.FromResult(0);
        }
        #endregion

        #region IUserLockoutStore Core
        public int GetAccessFailedCount(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null or empty.");

            return user.AccessFailedCount;
        }

        public bool GetLockoutEnabled(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null or empty.");

            return user.LockoutEnabled;
        }

        public DateTimeOffset GetLockoutEndDate(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null or empty.");

            return user.LockoutEnd.Value;
        }

        public int IncrementAccessFailedCount(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null or empty.");

            return ++user.AccessFailedCount;
        }

        public void ResetAccessFailedCount(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null or empty.");
            user.AccessFailedCount = 0;
        }

        public void SetLockoutEnabled(User user, bool enabled)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null or empty.");
            user.LockoutEnabled = enabled;
        }

        public void SetLockoutEndDate(User user, DateTimeOffset? lockoutEnd)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null or empty.");
            user.LockoutEnd = lockoutEnd;
        }
        #endregion

        #region IUserTwoFactorStore
        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task.FromResult(GetTwoFactorEnabled(user));
        }

        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            SetTwoFactorEnabled(user, enabled);
            return Task.FromResult(0);
        }

        #endregion

        #region IUserTwoFactorStore Core
        public bool GetTwoFactorEnabled(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null or empty.");

            return user.TwoFactorEnabled;
        }

        public void SetTwoFactorEnabled(User user, bool enabled)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null or empty.");

            user.TwoFactorEnabled = enabled;
        }

        #endregion

        #region IUserRoleStore

        public Task AddToRoleAsync(User user, string roleName)
        {
            AddToRole(user, roleName);
            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            return Task.FromResult(GetRoles(user));
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            return Task.FromResult(IsInRole(user, roleName));
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            RemoveFromRole(user, roleName);
            return Task.FromResult(0);
        }

        #endregion

        #region IUserRoleStore core

        public void AddToRole(User user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Value Cannot Be Null Or Empty", "roleName");

            var roleConstraints = new QueryConstraints<IdentityRole>()
                .Where(r => r.Name == roleName);
            var roleEntity = queryRepository.SingleOrDefault<IdentityRole>(roleConstraints);
            if (roleEntity == null)
            {
                throw new EntityNotFoundException(roleName, string.Format("'{0}' is not a valide Role", roleName));
            }
            user.Roles.Add(roleEntity);
        }

        public IList<string> GetRoles(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return user.Roles.Select(r => r.Name).ToList();
        }

        public bool IsInRole(User user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Value Cannot Be Null Or Empty", "roleName");

            return user.Roles.Any(r => r.Name == roleName);
        }

        public void RemoveFromRole(User user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Value Cannot Be Null Or Empty", "roleName");

            var role = user.Roles.FirstOrDefault(r => r.Name == roleName);
            if (role != null) user.Roles.Remove(role);
        }

        #endregion

    }

}