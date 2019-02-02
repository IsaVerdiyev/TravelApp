namespace TravelAppInfrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCityDestinationEntity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cities", "TripId", "dbo.Trips");
            DropIndex("dbo.Cities", new[] { "TripId" });
            CreateTable(
                "dbo.DestinationCityInTrips",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderNumber = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        TripId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.Trips", t => t.TripId, cascadeDelete: true)
                .Index(t => t.CityId)
                .Index(t => t.TripId);
            
            DropColumn("dbo.Cities", "TripId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cities", "TripId", c => c.Int(nullable: false));
            DropForeignKey("dbo.DestinationCityInTrips", "TripId", "dbo.Trips");
            DropForeignKey("dbo.DestinationCityInTrips", "CityId", "dbo.Cities");
            DropIndex("dbo.DestinationCityInTrips", new[] { "TripId" });
            DropIndex("dbo.DestinationCityInTrips", new[] { "CityId" });
            DropTable("dbo.DestinationCityInTrips");
            CreateIndex("dbo.Cities", "TripId");
            AddForeignKey("dbo.Cities", "TripId", "dbo.Trips", "Id", cascadeDelete: true);
        }
    }
}
