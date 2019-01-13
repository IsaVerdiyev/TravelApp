namespace TravelAppInfrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FullName = c.String(),
                        PictureUrl = c.String(),
                        Currency = c.String(),
                        Language = c.String(),
                        TripId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trips", t => t.TripId, cascadeDelete: true)
                .Index(t => t.TripId);
            
            CreateTable(
                "dbo.CityCoordinates",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Longitude = c.Single(nullable: false),
                        Latitude = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Trips",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DepartureDate = c.DateTime(nullable: false),
                        ArriavalDate = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ToDoItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Done = c.Boolean(nullable: false),
                        TripId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trips", t => t.TripId, cascadeDelete: true)
                .Index(t => t.TripId);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(),
                        TripId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trips", t => t.TripId, cascadeDelete: true)
                .Index(t => t.TripId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NickName = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trips", "UserId", "dbo.Users");
            DropForeignKey("dbo.Tickets", "TripId", "dbo.Trips");
            DropForeignKey("dbo.Cities", "TripId", "dbo.Trips");
            DropForeignKey("dbo.ToDoItems", "TripId", "dbo.Trips");
            DropForeignKey("dbo.CityCoordinates", "Id", "dbo.Cities");
            DropIndex("dbo.Tickets", new[] { "TripId" });
            DropIndex("dbo.ToDoItems", new[] { "TripId" });
            DropIndex("dbo.Trips", new[] { "UserId" });
            DropIndex("dbo.CityCoordinates", new[] { "Id" });
            DropIndex("dbo.Cities", new[] { "TripId" });
            DropTable("dbo.Users");
            DropTable("dbo.Tickets");
            DropTable("dbo.ToDoItems");
            DropTable("dbo.Trips");
            DropTable("dbo.CityCoordinates");
            DropTable("dbo.Cities");
        }
    }
}
