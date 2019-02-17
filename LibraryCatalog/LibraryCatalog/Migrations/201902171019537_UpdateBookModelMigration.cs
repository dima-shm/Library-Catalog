namespace LibraryCatalog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBookModelMigration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Books", "Price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "Price", c => c.Int(nullable: false));
        }
    }
}
