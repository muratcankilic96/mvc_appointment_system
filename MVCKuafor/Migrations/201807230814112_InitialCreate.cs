namespace MVCKuafor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerModels",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    Surname = c.String(nullable: false),
                    Username = c.String(nullable: false),
                    Email = c.String(nullable: false),
                    Password = c.String(nullable: false),
                    RegisterDate = c.DateTime(nullable: false),
                    isAdmin = c.Boolean(nullable: false)
                })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CustomerModels");
        }
    }
}
