namespace SavingDirectoryStructureByUsingNestedSetModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFileType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DirectoryTreeMap", "FileType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DirectoryTreeMap", "FileType");
        }
    }
}
