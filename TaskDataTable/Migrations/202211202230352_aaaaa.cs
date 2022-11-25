namespace TaskDataTable.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aaaaa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CusId = c.String(),
                        CusName = c.String(),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CustomerInfoes");
        }
    }
}
