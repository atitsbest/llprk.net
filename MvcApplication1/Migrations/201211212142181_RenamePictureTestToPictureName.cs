namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamePictureTestToPictureName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pictures", "Name", c => c.String());
            AddColumn("dbo.Pictures", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Pictures", "Test");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pictures", "Test", c => c.String());
            DropColumn("dbo.Pictures", "Discriminator");
            DropColumn("dbo.Pictures", "Name");
        }
    }
}
