namespace MVCGrid.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstM : DbMigration
    {
        public override void Up()
        {
         
            AlterColumn("dbo.WorkersSmall", "FirstName", c => c.String(nullable: false, maxLength: 50, fixedLength: true));
            AlterColumn("dbo.WorkersSmall", "LastName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.WorkersSmall", "Prefix", c => c.String(nullable: false, maxLength: 5, fixedLength: true));
            AlterColumn("dbo.WorkersSmall", "Position", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.WorkersSmall", "BirthDate", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
            AlterColumn("dbo.WorkersSmall", "Notes", c => c.String(maxLength: 500));
            AlterColumn("dbo.WorkersSmall", "Address", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.WorkersSmall", "Salary", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WorkersSmall", "Salary", c => c.Decimal(nullable: false, precision: 19, scale: 4));
            AlterColumn("dbo.WorkersSmall", "Address", c => c.String());
            AlterColumn("dbo.WorkersSmall", "Notes", c => c.String());
            AlterColumn("dbo.WorkersSmall", "BirthDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.WorkersSmall", "Position", c => c.String());
            AlterColumn("dbo.WorkersSmall", "Prefix", c => c.String());
            AlterColumn("dbo.WorkersSmall", "LastName", c => c.String());
            AlterColumn("dbo.WorkersSmall", "FirstName", c => c.String());
            RenameTable(name: "dbo.WorkersSmall", newName: "WorkerSmalls");
        }
    }
}
