namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Salutation = c.String(nullable: false),
                        Firstname = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Address1 = c.String(),
                        Address2 = c.String(nullable: false),
                        Zip = c.String(nullable: false),
                        City = c.String(nullable: false),
                        CountryCode = c.String(nullable: false),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Qty = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.OrderLines", new[] { "ProductId" });
            DropIndex("dbo.OrderLines", new[] { "OrderId" });
            DropForeignKey("dbo.OrderLines", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderLines", "ProductId", "dbo.Products");
            DropTable("dbo.OrderLines");
            DropTable("dbo.Orders");
        }
    }
}
