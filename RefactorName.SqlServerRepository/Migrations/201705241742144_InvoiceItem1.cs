namespace RefactorName.SqlServerRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceItem1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InvoiceDetails", "InvoiceId", "dbo.Invoices");
            AddForeignKey("dbo.InvoiceDetails", "InvoiceId", "dbo.Invoices", "InvoiceId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvoiceDetails", "InvoiceId", "dbo.Invoices");
            AddForeignKey("dbo.InvoiceDetails", "InvoiceId", "dbo.Invoices", "InvoiceId");
        }
    }
}
