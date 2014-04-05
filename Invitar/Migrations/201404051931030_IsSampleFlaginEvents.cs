namespace Invitar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsSampleFlaginEvents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "IsSample", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "IsSample");
        }
    }
}
