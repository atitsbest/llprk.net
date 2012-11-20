namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameCategoryId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Category_Id", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "Category_Id" });
            RenameColumn(table: "dbo.Products", name: "Category_Id", newName: "CategoryId");
            AddForeignKey("dbo.Products", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
            CreateIndex("dbo.Products", "CategoryId");
            DropColumn("dbo.Products", "CatgoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "CatgoryId", c => c.Int(nullable: false));
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            RenameColumn(table: "dbo.Products", name: "CategoryId", newName: "Category_Id");
            CreateIndex("dbo.Products", "Category_Id");
            AddForeignKey("dbo.Products", "Category_Id", "dbo.Categories", "Id");
        }
    }
}
