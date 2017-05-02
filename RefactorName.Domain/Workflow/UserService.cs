using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using RefactorName.Core;
using RefactorName.Core.SearchEntities;
using RefactorName.RepositoryInterface;
using RefactorName.RepositoryInterface.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RefactorName.Domain
{
    public class UserService : UserManager<User, int>
    {
        public static UserService Obj { get; private set; }

        private static IIdentityUserRepository identityUserRepository;

        private static IGenericQueryRepository queryRepository;

        static UserService()
        {
            Obj = new UserService(RepositoryFactory.Create<IIdentityUserRepository>("IdentityUserRepository"));
            ConfigureManager(Obj);
        }

        private static void ConfigureManager(UserService entity)
        {
            // Configure validation logic for usernames
            entity.UserValidator = new UserValidator<User, int>(entity)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };

            // Configure validation logic for passwords
            entity.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            entity.UserLockoutEnabledByDefault = false;
            entity.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            entity.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            //Obj.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User, int>
            //{
            //    MessageFormat = "Your security code is {0}"
            //});
            //Obj.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User, int>
            //{
            //    Subject = "Security Code",
            //    BodyFormat = "Your security code is {0}"
            //});
            //Obj.EmailService = new EmailService();
            //Obj.SmsService = new SmsService();

            //var dataProtectionProvider = options.DataProtectionProvider;
            //if (dataProtectionProvider != null)
            //{
            //    Obj.UserTokenProvider =
            //        new DataProtectorTokenProvider<User, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            //}
        }

        public Dictionary<string, string> GetAllUsersDictionary()
        {
            var result = new Dictionary<string, string>();

            var constraints = new QueryConstraints<User>()
                .Page(1, int.MaxValue)
                .Where(c => true);

            foreach (User user in queryRepository.Find(constraints).Items.ToList())
            {
                result.Add(user.Id.ToString(), user.FullName);
            }

            return result;
        }

        private UserService(IIdentityUserRepository store)
            : base(store)
        {
            queryRepository = RepositoryFactory.CreateQueryRepository();
            identityUserRepository = store;
        }

        public static UserService Create(IdentityFactoryOptions<UserService> options, IOwinContext context)
        {
            var newUserService = new UserService(identityUserRepository);
            ConfigureManager(newUserService);
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                newUserService.UserTokenProvider =
                    new DataProtectorTokenProvider<User, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return newUserService;
        }

        public IQueryResult<User> Find(UserSearchCriteria userSearchCriteria)
        {
            if (userSearchCriteria == null)
                throw new ArgumentNullException("userSearchCriteria", "must not be null.");

            var constraints = new QueryConstraints<User>()
                .Page(userSearchCriteria.PageNumber, userSearchCriteria.PageSize)
                .Where(c => true);

            if (userSearchCriteria.IsActive.HasValue)
                constraints.AndAlso(c => c.IsActive == userSearchCriteria.IsActive.Value);

            if (!string.IsNullOrEmpty(userSearchCriteria.FullName))
                constraints.AndAlso(c => c.FullName.Contains(userSearchCriteria.FullName));

            if (!string.IsNullOrEmpty(userSearchCriteria.PhoneNumber))
                constraints.AndAlso(c => c.PhoneNumber.Contains(userSearchCriteria.PhoneNumber));


            if (!string.IsNullOrEmpty(userSearchCriteria.UserName))
                constraints.AndAlso(c => c.UserName.Contains(userSearchCriteria.UserName));

            if (!string.IsNullOrEmpty(userSearchCriteria.Email))
                constraints.AndAlso(c => c.Email.Contains(userSearchCriteria.Email));


            if (userSearchCriteria.SortDirection == WebGridSortOrder.Ascending)
                constraints.SortBy(userSearchCriteria.Sort);
            else
                constraints.SortByDescending(userSearchCriteria.Sort);


            return queryRepository.Find(constraints);
        }

        public IQueryResult<User> FindAdmins(UserSearchCriteria userSearchCriteria)
        {
            if (userSearchCriteria == null)
                throw new ArgumentNullException("userSearchCriteria", "must not be null.");

            var constraints = new QueryConstraints<User>()
                .Page(userSearchCriteria.PageNumber, userSearchCriteria.PageSize)
                .Where(c => true);

            if (userSearchCriteria.IsActive.HasValue)
                constraints.AndAlso(c => c.IsActive == userSearchCriteria.IsActive.Value);

            if (!string.IsNullOrEmpty(userSearchCriteria.FullName))
                constraints.AndAlso(c => c.FullName.Contains(userSearchCriteria.FullName));

            if (!string.IsNullOrEmpty(userSearchCriteria.PhoneNumber))
                constraints.AndAlso(c => c.PhoneNumber.Contains(userSearchCriteria.PhoneNumber));


            if (!string.IsNullOrEmpty(userSearchCriteria.UserName))
                constraints.AndAlso(c => c.UserName.Contains(userSearchCriteria.UserName));

            if (!string.IsNullOrEmpty(userSearchCriteria.Email))
                constraints.AndAlso(c => c.Email.Contains(userSearchCriteria.Email));


            if (userSearchCriteria.SortDirection == WebGridSortOrder.Ascending)
                constraints.SortBy(userSearchCriteria.Sort);
            else
                constraints.SortByDescending(userSearchCriteria.Sort);


            return queryRepository.Find(constraints);
        }

        public IQueryResult<User> FindUsers(UserSearchCriteria userSearchCriteria)
        {
            if (userSearchCriteria == null)
                throw new ArgumentNullException("userSearchCriteria", "must not be null.");

            //var constraints = new QueryConstraints<Core.Workflow.Action>().SortByDescending(a => a.ActionId)
            //    .IncludePath(a => a.ActionType)
            //    .IncludePath(a => a.Process)
            //    .Page(ActionSearchCriteria.PageNumber, ActionSearchCriteria.PageSize).Where(a => a.ActionId > 0);


            var constraints = new QueryConstraints<User>().SortByDescending(a => a.Id)
                .Page(userSearchCriteria.PageNumber, userSearchCriteria.PageSize);

            if (userSearchCriteria.IsActive.HasValue)
                constraints.AndAlso(c => c.IsActive == userSearchCriteria.IsActive.Value);

            if (!string.IsNullOrEmpty(userSearchCriteria.FullName))
                constraints.AndAlso(c => c.FullName.Contains(userSearchCriteria.FullName));

            if (!string.IsNullOrEmpty(userSearchCriteria.PhoneNumber))
                constraints.AndAlso(c => c.PhoneNumber.Contains(userSearchCriteria.PhoneNumber));

            if (!string.IsNullOrEmpty(userSearchCriteria.UserName))
                constraints.AndAlso(c => c.UserName.Contains(userSearchCriteria.UserName));

            if (!string.IsNullOrEmpty(userSearchCriteria.Email))
                constraints.AndAlso(c => c.Email.Contains(userSearchCriteria.Email));

            if (userSearchCriteria.GroupId > 0)
                constraints.AndAlso(c => c.Groups.Any(g => g.GroupId == userSearchCriteria.GroupId));

            //if (userSearchCriteria.SortDirection == WebGridSortOrder.Ascending)
            //    constraints.SortBy(userSearchCriteria.Sort);
            //else
            //    constraints.SortByDescending(userSearchCriteria.Sort);


            return queryRepository.Find(constraints);
        }

        public List<User> GetAll()
        {
            var constraints = new QueryConstraints<User>()
                .Page(1, int.MaxValue)
                .Where(u => u.PasswordHash == null);

            return queryRepository.Find(constraints).Items.ToList();
        }

        public IQueryResult<User> GetActiveUsers()
        {
            var constraints = new QueryConstraints<User>()
                .Page(1, int.MaxValue)
                .Where(c => c.IsActive);

            return queryRepository.Find(constraints);
        }

        public IQueryResult<User> GetAllUsers()
        {
            var constraints = new QueryConstraints<User>()
                .Page(1, int.MaxValue)
                .Where(c => true);

            return queryRepository.Find(constraints);
        }

        public User Create(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null.");

            //if (!Thread.CurrentPrincipal.IsInRole(RoleNames.SuperAdministrator))
            //throw new PermissionException("You have no permission to execute this operation.", RoleNames.SuperAdministrator);

            //if (!user.Validate())
            //    throw new ValidationException("Business Entity has invalid information.", user.ValidationResults, ErrorCode.InvalidData);

            if (!user.IsUserNameUnique(true))
                throw new BusinessRuleException("اسم المستخدم موجود مسبقاً", ErrorCode.NotUnique);

            var result = identityUserRepository.CreateAsync(user);

            if (result.Exception != null)
            {
                if (result.Exception.InnerException != null)
                    throw new RepositoryException(result.Exception.InnerException.Message, ErrorCode.DatabaseError);
                else
                    throw new RepositoryException(result.Exception.Message, ErrorCode.DatabaseError);
            }
            return FindByName(user.UserName);
        }

        public override System.Threading.Tasks.Task<IdentityResult> CreateAsync(User user)
        {
            //if (user.IsADUser && !user.IsValidADUser())
            //    throw new BusinessRuleException("المستخدم غير موجود في السجل النشط!");
            return base.CreateAsync(user);
        }

        public User Create(User user, string password)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null.");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password", "must not be null.");

            //if (!Thread.CurrentPrincipal.IsInRole(RoleNames.SuperAdministrator))
            //throw new PermissionException("You have no permission to execute this operation.", RoleNames.SuperAdministrator);

            //if (!user.Validate())
            //    throw new ValidationException("Business Entity has invalid information.", user.ValidationResults, ErrorCode.InvalidData);

            if (!user.IsUserNameUnique(true))
                throw new BusinessRuleException("اسم المستخدم موجود مسبقاً", ErrorCode.NotUnique);

            var result = CreateAsync(user, password);

            if (result.Exception != null)
            {
                if (result.Exception.InnerException != null)
                    throw new RepositoryException(result.Exception.InnerException.Message, ErrorCode.DatabaseError);
                else
                    throw new RepositoryException(result.Exception.Message, ErrorCode.DatabaseError);
            }
            else if (result.Result.Errors.Any())
                throw new BusinessRuleException(string.Join(",", result.Result.Errors), ErrorCode.IdentityUserCreateError);

            return identityUserRepository.FindByNameAsync(user.UserName).Result;
        }

        public User FindByName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName", "must not be null or empty.");

            var constraints = new QueryConstraints<User>()
                .IncludePath(u=>u.Roles)
                .Where(x => x.UserName == userName);

            return queryRepository.SingleOrDefault(constraints);
        }

        public User FindByEmail(string Email)
        {
            if (string.IsNullOrEmpty(Email))
                throw new ArgumentNullException("Email", "must not be null or empty.");

            var constraints = new QueryConstraints<User>()
                .Where(x => x.Email == Email);

            return queryRepository.SingleOrDefault(constraints);
        }

        public User FindById(int userId)
        {
            if (userId == 0)
                throw new ArgumentNullException("userID", "must not be null.");

            var constraints = new QueryConstraints<User>()
                .Where(x => x.Id == userId);

            return queryRepository.SingleOrDefault(constraints);
        }

        public User Update(User user)
        {
            if (user == null)
                throw new ArgumentNullException("entity", "must not be null.");

            //if (!Thread.CurrentPrincipal.IsInRole(RoleNames.ManageUsers))
            //    throw new PermissionException("You have no permission to execute current operation.", RoleNames.ManageUsers);

            //if (!user.Validate())
            //    throw new ValidationException("Some data are not valid", user.ValidationResults, ErrorCode.InvalidData);

            var result = identityUserRepository.UpdateAsync(user);

            if (result.Exception != null)
            {
                if (result.Exception.InnerException != null)
                    throw new RepositoryException(result.Exception.InnerException.Message, ErrorCode.DatabaseError);
                else
                    throw new RepositoryException(result.Exception.Message, ErrorCode.DatabaseError);
            }
            return FindById(user.Id);
        }
    }

}