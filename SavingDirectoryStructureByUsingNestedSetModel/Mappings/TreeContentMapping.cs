using SavingDirectoryStructureByUsingNestedSetModel.Models;
using System.Data.Entity.ModelConfiguration;


namespace SavingDirectoryStructureByUsingNestedSetModel.Mappings
{
    public class TreeContentMapping : EntityTypeConfiguration<TreeContent>
    {
        public TreeContentMapping()
        {
            // Primary Key
            HasKey(t => t.Id);
            // Table & Column Mappings
            ToTable("TreeContent");
        }
    }
}
