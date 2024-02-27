namespace DatabaseConnectionAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseCreationfromModelCodeFirstApporach : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IdentityDBs",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: true),
                        CreatedAt = c.DateTime(nullable: false),
                        LastLoginAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.IdentityDBs");
        }
    }
}
