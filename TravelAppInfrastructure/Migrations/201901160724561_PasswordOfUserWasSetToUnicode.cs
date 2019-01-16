namespace TravelAppInfrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PasswordOfUserWasSetToUnicode : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 40));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 40, unicode: false));
        }
    }
}
