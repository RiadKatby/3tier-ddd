using RefactorName.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using RefactorName.Core;
using System.Threading;
using RefactorName.RepositoryInterface.Queries;

namespace RefactorName.Domain
{
    public class RoleService
        : IRoleStore<IdentityRole, int>
    {
        public static RoleService Obj { get; private set; }

        private static IGenericQueryRepository queryRepository;
        private static IGenericRepository repository;

        static RoleService()
        {
            Obj = new RoleService();
        }

        private RoleService()
        {
            queryRepository = RepositoryFactory.CreateQueryRepository();
            repository = RepositoryFactory.CreateRepository();
        }


        public List<IdentityRole> GetByNames(string[] roleNames)
        {
            if (roleNames == null)
                return new List<IdentityRole>();

            var constraints = new QueryConstraints<IdentityRole>()
                .Page(1, int.MaxValue)
                .SortBy("Name")
                .Where(x => roleNames.Contains(x.Name));

            return queryRepository.Find(constraints).Items.ToList();
        }

        #region IRoleStore
        public Task CreateAsync(IdentityRole role)
        {
            return Task.FromResult(Create(role));
        }

        public Task DeleteAsync(IdentityRole role)
        {
            return Task.FromResult(Delete(role));
        }

        public Task<IdentityRole> FindByIdAsync(int roleId)
        {
            return Task.FromResult(FindById(roleId));
        }

        public Task<IdentityRole> FindByNameAsync(string roleName)
        {
            return Task.FromResult(FindByName(roleName));
        }

        public Task UpdateAsync(IdentityRole role)
        {
            return Task.FromResult(Update(role));
        }

        public void Dispose()
        {
        }


        #endregion

        #region IRoleStore Core
        public IdentityRole Create(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role", "must not be null.");

            if (!Thread.CurrentPrincipal.IsInRole(RoleNames.SuperAdministrator))
                throw new PermissionException("You have no permission to execute this operation.", RoleNames.SuperAdministrator);

            return repository.Create<IdentityRole>(role);
        }

        public bool Delete(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role", "must not be null.");

            //if (!Thread.CurrentPrincipal.IsInRole(RoleNames.ManageUsers))
            //    throw new PermissionException("You have no permission to execute this operation.", RoleNames.ManageUsers);

            return repository.Delete<IdentityRole>(role);
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
        #endregion

    }
}
