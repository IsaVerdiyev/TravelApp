namespace TravelAppInfrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class entityPropertyRestrictions : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cities", "Name", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Cities", "FullName", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Cities", "PictureUrl", c => c.String(maxLength: 60));
            AlterColumn("dbo.Cities", "Currency", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Cities", "Language", c => c.String(maxLength: 30));
            AlterColumn("dbo.ToDoItems", "Name", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Tickets", "ImagePath", c => c.String(maxLength: 60));
            AlterColumn("dbo.Users", "NickName", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 40, unicode: false));
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 40, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Email", c => c.String());
            AlterColumn("dbo.Users", "Password", c => c.String());
            AlterColumn("dbo.Users", "NickName", c => c.String());
            AlterColumn("dbo.Tickets", "ImagePath", c => c.String());
            AlterColumn("dbo.ToDoItems", "Name", c => c.String());
            AlterColumn("dbo.Cities", "Language", c => c.String());
            AlterColumn("dbo.Cities", "Currency", c => c.String());
            AlterColumn("dbo.Cities", "PictureUrl", c => c.String());
            AlterColumn("dbo.Cities", "FullName", c => c.String());
            AlterColumn("dbo.Cities", "Name", c => c.String());
        }
    }
}
