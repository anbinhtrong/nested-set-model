using SavingDirectoryStructureByUsingNestedSetModel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingDirectoryStructureByUsingNestedSetModel.Services
{
    public class TreeContentService
    {
        private readonly DbContext _context;
        private readonly DbSet<TreeContent> _treeContentSet;

        public TreeContentService()
        {
            _context = new TreeContext();
            _treeContentSet = _context.Set<TreeContent>();
        }

        public void InitRoot()
        {
            var query = "Select top 1 * from TreeContent";
            var result = _treeContentSet.SqlQuery(query).FirstOrDefault();
            if (result == null)
            {
                var newNode = new TreeContent
                {
                    NodeId = 1,
                    Name = "Home"
                };
                _treeContentSet.Add(newNode);
                _context.SaveChanges();
            }
        }

        
    }
}
