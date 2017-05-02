using RefactorName.Core;
using Microsoft.AspNet.Identity;

namespace RefactorName.RepositoryInterface
{
    public interface IIdentityRoleRepository : IRoleStore<IdentityRole, int>
    {

    }
}
