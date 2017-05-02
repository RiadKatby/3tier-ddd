using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.SqlServerRepository.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<RefactorNameDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RefactorNameDbContext context)
        {
            
        }
    }
}
