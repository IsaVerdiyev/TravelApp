namespace TravelAppInfrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNamePropertyInTrip : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trips", "Name", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trips", "Name");
        }
    }
}
