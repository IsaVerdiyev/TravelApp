namespace TravelAppInfrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CityCoordinates", "Id", "dbo.Cities");
            AddForeignKey("dbo.CityCoordinates", "Id", "dbo.Cities", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CityCoordinates", "Id", "dbo.Cities");
            AddForeignKey("dbo.CityCoordinates", "Id", "dbo.Cities", "Id");
        }
    }
}
