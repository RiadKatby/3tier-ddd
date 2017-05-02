using RefactorName.Core;

namespace RefactorName.RepositoryInterface
{
    public interface IActiveDirectoryRepository
    {
        bool AuthenticateUser(string userName, string password);
        ActiveDirectoryUserInfo GetUserInfo(string userName);
    }
}
