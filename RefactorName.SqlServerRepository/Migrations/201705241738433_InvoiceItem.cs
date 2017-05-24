namespace RefactorName.SqlServerRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ItemId);
            
            AddColumn("dbo.InvoiceDetails", "ItemId", c => c.Int());
            CreateIndex("dbo.InvoiceDetails", "ItemId");
            AddForeignKey("dbo.InvoiceDetails", "ItemId", "dbo.Items", "ItemId");
            DropColumn("dbo.InvoiceDetails", "ItemName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InvoiceDetails", "ItemName", c => c.String());
            DropForeignKey("dbo.InvoiceDetails", "ItemId", "dbo.Items");
            DropIndex("dbo.InvoiceDetails", new[] { "ItemId" });
            DropColumn("dbo.InvoiceDetails", "ItemId");
            DropTable("dbo.Items");
        }
    }
}
