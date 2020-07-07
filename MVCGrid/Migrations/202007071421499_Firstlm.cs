namespace MVCGrid.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Firstlm : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WorkersSmall", "Prefix", c => c.String(nullable: false, maxLength: 5));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WorkersSmall", "Prefix", c => c.String(nullable: false, maxLength: 5, fixedLength: true));
        }
    }
}
