namespace MVCGrid.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Firstl : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WorkersSmall", "FirstName", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WorkersSmall", "FirstName", c => c.String(nullable: false, maxLength: 20, fixedLength: true));
        }
    }
}
