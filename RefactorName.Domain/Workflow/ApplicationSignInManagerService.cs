using RefactorName.Core;
using RefactorName.RepositoryInterface;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RefactorName.Domain
{
    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<User, int>
    {
        //private static IActiveDirectoryRepository activeDirectoryRepository=new ;
        public ApplicationSignInManager(UserService userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
            //activeDirectoryRepository = RepositoryFactory.CreateWebSvc<IActiveDirectoryRepository>("ActiveDirectoryRepository");
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((UserService)UserManager);
        }

        //public virtual ActiveDirectoryUserInfo ActiveDirectoryUserGetInfo(User user)
        //{
        //    return activeDirectoryRepository.GetUserInfo(user.UserName);
        //}

        //public virtual ActiveDirectoryUserInfo ActiveDirectoryUserGetInfo(string userName)
        //{
        //    return activeDirectoryRepository.GetUserInfo(userName);
        //}

        //public virtual SignInStatus ActiveDirectorySignInAsync(User user, string pass, bool isPersistent, bool rememberBrowser)
        //{
        //    var adUser = activeDirectoryRepository.AuthenticateUser(user.UserName, pass);

        //    if (!adUser)
        //        return SignInStatus.Failure;
        //    //if (!adUser.Enabled)
        //    //    return SignInStatus.Failure;

        //    var userIdentity = Task.FromResult(CreateUserIdentityAsync(user).Result).Result;

        //    // Clear any partial cookies from external or two factor partial sign ins
        //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
        //    if (rememberBrowser)
        //    {
        //        var rememberBrowserIdentity = AuthenticationManager.CreateTwoFactorRememberBrowserIdentity(ConvertIdToString(user.Id));
        //        AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, userIdentity, rememberBrowserIdentity);
        //    }
        //    else
        //    {
        //        AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, userIdentity);
        //    }

        //    return SignInStatus.Success;
        //}

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(UserService.Obj, context.Authentication);
        }
    }
}
