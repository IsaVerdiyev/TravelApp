namespace TravelAppInfrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreasedLengthOfStrings : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cities", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Cities", "FullName", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Cities", "Language", c => c.String(maxLength: 50));
            AlterColumn("dbo.ToDoItems", "Name", c => c.String(nullable: false, maxLength: 70));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ToDoItems", "Name", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Cities", "Language", c => c.String(maxLength: 30));
            AlterColumn("dbo.Cities", "FullName", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Cities", "Name", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
