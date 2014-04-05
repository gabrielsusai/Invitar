namespace Invitar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdinEvents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String(nullable: false));
            CreateIndex("dbo.Events", "UserId");
            AddForeignKey("dbo.Events", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Events", new[] { "UserId" });
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String());
            DropColumn("dbo.Events", "UserId");
        }
    }
}
