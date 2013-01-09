namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAvailableToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Available", c => c.Int(nullable: false, defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Available");
        }
    }
}
