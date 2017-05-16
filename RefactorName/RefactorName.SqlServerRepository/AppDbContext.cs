using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using RefactorName.Core;

namespace RefactorName.SqlServerRepository
{
    public class AppDbContext : DbContext
    {
        public static AppDbContext Create()
        {
            return new AppDbContext();
        }
        #region Identity
        public DbSet<IdentityUserClaim> UserClaims { get; set; }
        public DbSet<IdentityUserLogin> UserLogins { get; set; }

        public DbSet<IdentityRole> Roles { get; set; }
        public DbSet<IdentityRoleClaim> RoleClaims { get; set; }
        #endregion

        public DbSet<IdentityUser> Users { get; set; }

        public AppDbContext()
            : base("CnnStr")
        {
            base.Configuration.ProxyCreationEnabled = false;

            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
#if DEBUG
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
#endif
            Database.SetInitializer<AppDbContext>(new CreateDatabaseIfNotExists<AppDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            BaseEntityMap(modelBuilder);

            #region Identity

            modelBuilder.Entity<IdentityUserClaim>()
                .HasKey(uc => uc.Id)
                .ToTable("UserClaims");

            modelBuilder.Entity<IdentityRoleClaim>()
                .HasKey(rc => rc.Id)
                .ToTable("RoleClaims");

            modelBuilder.Entity<IdentityUserLogin>()
             .HasKey(l => new { l.LoginProvider, l.ProviderKey })
                .ToTable("UserLogins");

            modelBuilder.Entity<IdentityRole>()
                .ToTable("Roles")
                .Property(r => r.ConcurrencyStamp).IsRowVersion();
            modelBuilder.Entity<IdentityRole>().Property(u => u.Name).HasMaxLength(256);
            modelBuilder.Entity<IdentityRole>().HasMany(r => r.Claims).WithRequired().HasForeignKey(r => r.RoleId);

            modelBuilder.Entity<IdentityUser>()
                .ToTable("Users")
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .Map(x =>
                {
                    x.MapLeftKey("UserID");
                    x.MapRightKey("RoleID");
                    x.ToTable("UsersRoles");
                });
            modelBuilder.Entity<IdentityUser>().Property(u => u.ConcurrencyStamp).IsRowVersion();
            modelBuilder.Entity<IdentityUser>().Property(u => u.UserName).HasMaxLength(256);
            modelBuilder.Entity<IdentityUser>().Property(u => u.Email).HasMaxLength(256);
            modelBuilder.Entity<IdentityUser>().HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            modelBuilder.Entity<IdentityUser>().HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);


            #endregion

            base.OnModelCreating(modelBuilder);
        }

        private void BaseEntityMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOptional(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey(x => x.CreatedByID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasOptional(x => x.UpdatedBy)
                .WithMany()
                .HasForeignKey(x => x.UpdatedByID)
                .WillCascadeOnDelete(false);
        }
    }
}