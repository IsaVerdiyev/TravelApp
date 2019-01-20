namespace TravelAppInfrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PicturePathPropertiesLengthIncreasing : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cities", "PictureUrl", c => c.String(maxLength: 500));
            AlterColumn("dbo.Tickets", "ImagePath", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tickets", "ImagePath", c => c.String(maxLength: 60));
            AlterColumn("dbo.Cities", "PictureUrl", c => c.String(maxLength: 60));
        }
    }
}
