namespace DatabaseConnectionAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ordertable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    OrderRefID = c.Long(nullable: false),
                    BookID = c.Int(nullable: false),
                    BookName = c.String(nullable: false),
                    TotalBookCount = c.Int(nullable: false),
                    UserID = c.Int(nullable: false),
                    Username = c.String(nullable: false),
                    DateOfPurchase = c.DateTime(nullable: false),
                    TotalPrice = c.Double(nullable: false),
                })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.Orders");
        }
    }
}
