namespace TravelAppInfrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketEntityChanging : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Tickets", "ImagePath", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tickets", "ImagePath", c => c.String(maxLength: 500));
            DropColumn("dbo.Tickets", "Name");
        }
    }
}
