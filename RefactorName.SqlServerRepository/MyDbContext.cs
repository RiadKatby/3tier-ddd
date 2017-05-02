using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using RefactorName.Core;

namespace RefactorName.SqlServerRepository
{
    public class MyDbContext : DbContext
    {
        public static MyDbContext Create()
        {
            return new MyDbContext();
        }
        #region Identity
        public DbSet<IdentityUserClaim> UserClaims { get; set; }
        //public DbSet<IdentityUserLogin> UserLogins { get; set; }

        public DbSet<IdentityRole> Roles { get; set; }
        public DbSet<IdentityRoleClaim> RoleClaims { get; set; }
        #endregion

        #region Entities
        public DbSet<Process> Process { get; set; }

        public DbSet<State> State { get; set; }
        public DbSet<StateType> StateType { get; set; }

        public DbSet<Transition> Transition { get; set; }


        public DbSet<Field> Field { get; set; }
        public DbSet<FieldType> FieldType { get; set; }
        public DbSet<StateField> StateField { get; set; }

        public DbSet<Core.Action> Action { get; set; }
        public DbSet<ActionType> ActionType { get; set; }

        public DbSet<Activity> Activity { get; set; }
        public DbSet<ActivityType> ActivityType { get; set; }

        public DbSet<Request> Request { get; set; }
        public DbSet<RequestAction> RequestAction { get; set; }
        public DbSet<RequestData> RequestData { get; set; }
        public DbSet<RequestNote> RequestNote { get; set; }

        public DbSet<Group> Group { get; set; }
        public DbSet<Target> Target { get; set; }

        public DbSet<User> User { get; set; }

        #endregion
        //public DbSet<User> Users { get; set; }

        //public DbSet<IdentityUser> Users { get; set; }

        public MyDbContext() : base("DefaultConnection")
        {
            base.Configuration.ProxyCreationEnabled = false;
            base.Configuration.ValidateOnSaveEnabled = false;
            base.Configuration.LazyLoadingEnabled = false;

            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
#if DEBUG
            Database.Log = new Action<string>(s =>
            {
                string str = s.Length > 32766 ? s.Substring(0, 30000) : s;
                System.Diagnostics.Debug.WriteLine("{0}", (object)str);
            });
#endif
            Database.SetInitializer<MyDbContext>(new CreateDatabaseIfNotExists<MyDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Database.SetInitializer<MyDbContext>(null);
            //base.OnModelCreating(modelBuilder);

            BaseEntityMap(modelBuilder);

            //#region Identity

            //modelBuilder.Entity<IdentityUserClaim>()
            //    .HasKey(uc => uc.Id)
            //    .ToTable("UserClaims");

            //modelBuilder.Entity<IdentityRoleClaim>()
            //    .HasKey(rc => rc.Id)
            //    .ToTable("RoleClaims");

            //modelBuilder.Entity<IdentityUserLogin>()
            // .HasKey(l => new { l.LoginProvider, l.ProviderKey })
            //    .ToTable("UserLogins");

            //modelBuilder.Entity<IdentityRole>()
            //    .ToTable("Roles")
            //    .Property(r => r.ConcurrencyStamp).IsRowVersion();
            //modelBuilder.Entity<IdentityRole>().Property(u => u.Name).HasMaxLength(256);
            //modelBuilder.Entity<IdentityRole>().HasMany(r => r.Claims).WithRequired().HasForeignKey(r => r.RoleId);

            //modelBuilder.Entity<IdentityUser>()
            //    .ToTable("Users")
            //    .HasMany(u => u.Roles)
            //    .WithMany(r => r.Users)
            //    .Map(x =>
            //    {
            //        x.MapLeftKey("ID");
            //        x.MapRightKey("RoleID");
            //        x.ToTable("UsersRoles");
            //    });
            //modelBuilder.Entity<IdentityUser>().Property(u => u.ConcurrencyStamp).IsRowVersion();
            //modelBuilder.Entity<IdentityUser>().Property(u => u.UserName).HasMaxLength(256);
            //modelBuilder.Entity<IdentityUser>().Property(u => u.Email).HasMaxLength(256);
            //modelBuilder.Entity<IdentityUser>().HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            //modelBuilder.Entity<IdentityUser>().HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);


            //#endregion

            #region Identity

            modelBuilder.Entity<IdentityUserClaim>()
                .HasKey(uc => uc.Id)
                .ToTable("UserClaims");

            modelBuilder.Entity<IdentityRoleClaim>()
                .HasKey(rc => rc.Id)
                .ToTable("RoleClaims");

            modelBuilder.Entity<IdentityUserLogin>()
             .HasKey(l => new { l.LoginProvider, l.ProviderKey })
                .ToTable("Logins");

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
            modelBuilder.Entity<IdentityUser>().Property(u => u.UserName).HasMaxLength(256);
            modelBuilder.Entity<IdentityUser>().Property(u => u.ConcurrencyStamp).IsRowVersion();
            modelBuilder.Entity<IdentityUser>().Property(u => u.UserName).HasMaxLength(256);
            modelBuilder.Entity<IdentityUser>().Property(u => u.Email).HasMaxLength(256);
            modelBuilder.Entity<IdentityUser>().HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            modelBuilder.Entity<IdentityUser>().HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);

            #endregion


            #region Relationship
            modelBuilder.Entity<Transition>()
               .HasMany<Core.Action>(s => s.Actions)
               .WithMany(c => c.Transitions)
               .Map(cs =>
               {
                   cs.MapLeftKey("TransitionId");
                   cs.MapRightKey("ActionId");
                   cs.ToTable("TransitionActions");
               });
            modelBuilder.Entity<Transition>()
               .HasMany<Activity>(s => s.Activities)
               .WithMany(c => c.Transitions)
               .Map(cs =>
               {
                   cs.MapLeftKey("TransitionId");
                   cs.MapRightKey("ActivityId");
                   cs.ToTable("TransitionActivities");
               });
            modelBuilder.Entity<State>()
               .HasMany<Activity>(s => s.Activities)
               .WithMany(c => c.States)
               .Map(cs =>
               {
                   cs.MapLeftKey("StateId");
                   cs.MapRightKey("ActivityId");
                   cs.ToTable("StateActivities");
               });

            modelBuilder.Entity<User>()
           .HasMany<Group>(s => s.Groups)
           .WithMany(c => c.Users)
           .Map(cs =>
           {
               cs.MapLeftKey("Id");
               cs.MapRightKey("GroupId");
               cs.ToTable("UserGroups");
           });
            #endregion
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();


            base.OnModelCreating(modelBuilder);
        }

        private void BaseEntityMap(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>()
            //    .HasOptional(x => x.CreatedBy)
            //    .WithMany()
            //    .HasForeignKey(x => x.CreatedByID)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<User>()
            //    .HasOptional(x => x.UpdatedBy)
            //    .WithMany()
            //    .HasForeignKey(x => x.UpdatedByID)
            //    .WillCascadeOnDelete(false);
        }
    }
}