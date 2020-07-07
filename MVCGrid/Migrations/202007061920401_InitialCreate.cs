namespace MVCGrid.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkerSmalls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Prefix = c.String(),
                        Position = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Notes = c.String(),
                        Address = c.String(),
                        StateID = c.Int(nullable: false),
                        Salary = c.Decimal(nullable: false, precision: 19, scale: 4),
                        IsAlcoholic = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WorkerSmalls");
        }
    }
}
