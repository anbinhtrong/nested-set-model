using SavingDirectoryStructureByUsingNestedSetModel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingDirectoryStructureByUsingNestedSetModel.Mappings
{
    public class DirectoryTreeMapping : EntityTypeConfiguration<DirectoryTreeMap>
    {

        public DirectoryTreeMapping()
        {
            // Primary Key
            HasKey(t => t.Id);
            // Table & Column Mappings
            ToTable("DirectoryTreeMap");            
        }
    }
}
