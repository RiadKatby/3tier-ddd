using RefactorName.Core.Workflow;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.SqlServerRepository
{
    internal class AppDbContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<State>()
                .HasMany(x => x.Outgoing)
                .WithRequired(x => x.NextState)
                .HasForeignKey(x => x.NextStateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<State>()
                .HasMany(x => x.Ingoing)
                .WithRequired(x => x.CurrentState)
                .HasForeignKey(x => x.CurrentStateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Core.Workflow.Action>()
                .HasMany(x => x.Transitions)
                .WithMany(x => x.Actions)
                .Map(x =>
                {
                    x.MapLeftKey("TransitionId");
                    x.MapRightKey("ActionId");
                    x.ToTable("TransitionAction");
                });
        }
    }
}
