namespace DatabaseConnectionAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatingBookTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookDBs",
                c => new
                    {
                        BookID = c.Int(nullable: false, identity: true),
                        BookName = c.String(nullable: false),
                        BookAuthor = c.String(nullable: false),
                        BookDescription = c.String(nullable: false),
                        BookPrice = c.Int(nullable: false),
                        ReleasedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BookID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BookDBs");
        }
    }
}
