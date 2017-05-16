using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;

namespace RefactorName.Core
{

    /// <summary>
    ///     Represents a Role entity
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class IdentityRole : IRole<int>
    {
        public IdentityRole()
        {
            Users = new List<IdentityUser>();
            Claims = new List<IdentityRoleClaim>();
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="roleName"></param>
        public IdentityRole(string roleName)
            : this()
        {
            Name = roleName;
        }

        /// <summary>
        ///     Navigation property for users in the role
        /// </summary>
        public virtual ICollection<IdentityUser> Users { get; private set; }

        /// <summary>
        ///     Navigation property for claims in the role
        /// </summary>
        public virtual ICollection<IdentityRoleClaim> Claims { get; private set; }

        ///// <summary>
        /////     Role id
        ///// </summary>
        //public virtual int Id { get; set; }

        /// <summary>
        ///     Role name
        /// </summary>
        public virtual string Name { get; set; }
        public virtual string NormalizedName { get; set; }

        /// <summary>
        /// A random value that should change whenever a role is persisted to the store
        /// </summary>
        public virtual byte[] ConcurrencyStamp { get; set; }


        public virtual int Id { get; private set; }
    }
}
