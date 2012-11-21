namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnbekannteAenderunge : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Pictures", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pictures", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
