using SavingDirectoryStructureByUsingNestedSetModel.Mappings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingDirectoryStructureByUsingNestedSetModel.Models
{
    public class TreeContext: DbContext
    {

        public TreeContext() : base("webconnection")
        {

        }
        public TreeContext(string connectionString) : base(connectionString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DirectoryTreeMapping());
            modelBuilder.Configurations.Add(new TreeContentMapping());            
        }
    }
}
