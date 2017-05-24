namespace RefactorName.SqlServerRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceItem2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Items", "Name", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Items", "Name", c => c.String());
        }
    }
}
