using RefactorName.Core;
using RefactorName.RepositoryInterface;
using RefactorName.RepositoryInterface.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RefactorName.Domain
{
    public class RoleService
    {
        public static RoleService Obj { get; private set; }

        private static IGenericQueryRepository queryRepository;
        private static IGenericRepository repository;
        private static IIdentityRoleRepository roleRepository;

        static RoleService()
        {
            Obj = new RoleService();
        }

        private RoleService()
        {
            queryRepository = RepositoryFactory.CreateQueryRepository();
            repository = RepositoryFactory.CreateRepository();
            roleRepository = RepositoryFactory.Create<IIdentityRoleRepository>("IdentityRoleRepository");
        }


        public List<IdentityRole> GetByNames(string[] roleNames)
        {
            if (roleNames == null)
                return new List<IdentityRole>();

            var constraints = new QueryConstraints<IdentityRole>()
                .Page(1, int.MaxValue)
                .SortBy("Name")
                .Where(x => roleNames.Contains(x.Name));

            var result =  queryRepository.Find(constraints).Items.ToList();
            return result;
        }

        public IdentityRole Create(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role", "must not be null.");

            if (!Thread.CurrentPrincipal.IsInRole(RoleNames.SuperAdministrator))
                throw new PermissionException("You have no permission to execute this operation.", RoleNames.SuperAdministrator, ErrorCode.NotAuthorized);

            return repository.Create<IdentityRole>(role);
        }

        public IdentityRole FindById(int roleId)
        {
            if (roleId == 0)
                throw new ArgumentNullException("roleId", "must not be null.");

            var constraints = new QueryConstraints<IdentityRole>()
                .Where(x => x.Id == roleId);

            return queryRepository.SingleOrDefault(constraints);
        }

        public IdentityRole FindByName(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName", "must not be null or empty.");

            var constraints = new QueryConstraints<IdentityRole>()
                .Where(x => x.Name == roleName);

            return queryRepository.SingleOrDefault(constraints);
        }

        public IdentityRole Update(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role", "must not be null.");

            //if (!Thread.CurrentPrincipal.IsInRole(RoleNames.ManageUsers))
            //    throw new PermissionException("You have no permission to execute current operation.", RoleNames.ManageUsers);

            return repository.Update<IdentityRole>(role);
        }
    }
}
