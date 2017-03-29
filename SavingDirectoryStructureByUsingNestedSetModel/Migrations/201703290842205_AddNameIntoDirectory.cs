namespace SavingDirectoryStructureByUsingNestedSetModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameIntoDirectory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DirectoryTreeMap", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DirectoryTreeMap", "Name");
        }
    }
}
