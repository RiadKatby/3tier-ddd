namespace RefactorName.SqlServerRepository.Migrations
{
    using RefactorName.Core;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RefactorName.SqlServerRepository.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RefactorName.SqlServerRepository.AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //




            //Roles
            context.Roles.AddOrUpdate(
                r => r.Id, RoleNames.GetRolesWithCaptions().Select(r => new IdentityRole { Name = r.Key }).ToArray());
        }
    }
}
