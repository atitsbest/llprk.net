namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Pice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CatgoryId = c.Int(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.ProductPictures",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        Picture_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.Picture_Id })
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.Pictures", t => t.Picture_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.Picture_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ProductPictures", new[] { "Picture_Id" });
            DropIndex("dbo.ProductPictures", new[] { "Product_Id" });
            DropIndex("dbo.Products", new[] { "Category_Id" });
            DropForeignKey("dbo.ProductPictures", "Picture_Id", "dbo.Pictures");
            DropForeignKey("dbo.ProductPictures", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Products", "Category_Id", "dbo.Categories");
            DropTable("dbo.ProductPictures");
            DropTable("dbo.Products");
        }
    }
}
