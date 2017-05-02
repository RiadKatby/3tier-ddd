using RefactorName.Core;
using RefactorName.Core.Workflow;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.SqlServerRepository
{
    public class RefactorNameDbContext : DbContext
    {
        public RefactorNameDbContext() : base("CnnStr")
        {
            base.Configuration.ProxyCreationEnabled = false;
            base.Configuration.ValidateOnSaveEnabled = false;

            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;

            #if DEBUG
                        Database.Log = new Action<string>(s =>
                        {
                            string str = s.Length > 32766 ? s.Substring(0, 30000) : s;
                            System.Diagnostics.Debug.WriteLine("{0}", (object)str);
                        });
            #endif

            Database.SetInitializer<RefactorNameDbContext>(new CreateDatabaseIfNotExists<RefactorNameDbContext>());
        }
        public static RefactorNameDbContext Create()
        {
            return new RefactorNameDbContext();
        }

        #region Identity Entities
        public DbSet<User> User { get; set; }

        #endregion

        #region Workflow Entities
        public DbSet<Process> Process { get; set; }

        public DbSet<State> State { get; set; }
        public DbSet<StateType> StateType { get; set; }

        public DbSet<Transition> Transition { get; set; }

        public DbSet<Core.Workflow.Action> Action { get; set; }
        public DbSet<ActionType> ActionType { get; set; }

        public DbSet<Activity> Activity { get; set; }
        public DbSet<ActivityType> ActivityType { get; set; }

        public DbSet<Field> Field { get; set; }
        public DbSet<FieldType> FieldType { get; set; }
        public DbSet<StateField> StateField { get; set; }

        

        public DbSet<RequestAction> RequestAction { get; set; }
        public DbSet<RequestData> RequestData { get; set; }
        public DbSet<RequestNote> RequestNote { get; set; }

        public DbSet<Group> Group { get; set; }
        #endregion

        #region Entities
        public DbSet<Request> Request { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region Identity

            #endregion

            #region Entities

            #endregion

            BaseEntityMap(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void BaseEntityMap(DbModelBuilder modelBuilder)
        {
            
        }
    }
}
