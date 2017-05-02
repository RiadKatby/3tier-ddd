namespace RefactorName.SqlServerRepository.Migrations
{
    using Core;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RefactorName.SqlServerRepository.MyDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "RefactorName.SqlServerRepository.MyDbContext";
        }

        protected override void Seed(RefactorName.SqlServerRepository.MyDbContext context)
        {
            context.ActionType.AddOrUpdate(ActionType.Approve);
            context.ActionType.AddOrUpdate(ActionType.Cancel);
            context.ActionType.AddOrUpdate(ActionType.Deny);
            context.ActionType.AddOrUpdate(ActionType.Resolve);
            context.ActionType.AddOrUpdate(ActionType.Restart);

            context.ActivityType.AddOrUpdate(ActivityType.AddNote);
            context.ActivityType.AddOrUpdate(ActivityType.AddStakeholders);
            context.ActivityType.AddOrUpdate(ActivityType.RemoveStakeholders);
            context.ActivityType.AddOrUpdate(ActivityType.SendEmail);

            context.Target.AddOrUpdate(Target.Requester);
            context.Target.AddOrUpdate(Target.GroupMemebers);
            context.Target.AddOrUpdate(Target.Stakeholders);
            context.Target.AddOrUpdate(Target.ProcessAdmins);

            base.Seed(context);
        }
    }
}
