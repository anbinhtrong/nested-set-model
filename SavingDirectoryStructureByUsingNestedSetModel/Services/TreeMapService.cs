﻿using SavingDirectoryStructureByUsingNestedSetModel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingDirectoryStructureByUsingNestedSetModel.Services
{
    public class TreeMapService
    {
        private readonly DbContext _context;
        private readonly DbSet<DirectoryTreeMap> _directoryTreeMapSet;

        public TreeMapService()
        {
            _context = new TreeContext();
            _directoryTreeMapSet = _context.Set<DirectoryTreeMap>();
        }

        public void InitRoot()
        {
            var query = "Select top 1 * from DirectoryTreeMap";
            var result = _directoryTreeMapSet.SqlQuery(query).FirstOrDefault();
            if(result == null)
            {
                var newNode = new DirectoryTreeMap
                {
                    Lft = 1,
                    Rgt = 2,
                    ParentId = 0
                };
                _directoryTreeMapSet.Add(newNode);
                _context.SaveChanges();
            }
        }

        public void InsertNewNode(int parentNodeId, string nodeName, FileTypeEnum fileType)
        {
            _context.Database.ExecuteSqlCommand("Exec InsertNewNode @p0, @p1, @p2", parentNodeId, nodeName, fileType);
        }

        public List<DirectoryTreeMap> GetByName(string name)
        {
            var query = $"SELECT * FROM dbo.DirectoryTreeMap WHERE Name = '{name}'";
            var result = _directoryTreeMapSet.SqlQuery(query).ToList();
            return result;
        }

        public void DisplayRootTree()
        {
            // retrieve the left and right value of the $root node  
            var query = $"SELECT TOP 1 * FROM dbo.DirectoryTreeMap ORDER BY id;";
            var result1 = _directoryTreeMapSet.SqlQuery(query).AsNoTracking().FirstOrDefault();
            if(result1 != null)
            {
                DisplayTree(result1);
            }
        }

        public void DisplayTree(DirectoryTreeMap root)
        {            
           
            var right = new List<int>();

            // now, retrieve all descendants of the $root node  

            var query = "SELECT * FROM DirectoryTreeMap " + $"WHERE lft BETWEEN {root.Lft} AND {root.Rgt} ORDER BY [lft] ASC;";
            var children = _directoryTreeMapSet.SqlQuery(query).AsNoTracking().ToList();
            // display each row  
            foreach (var child in children)
            {
                // only check stack if there is one  
                if (right.Count > 0)
                {
                    // check if we should remove a node from the stack  
                    while (right[right.Count - 1] < child.Rgt)
                    {
                        right.RemoveAt(right.Count - 1);
                    }
                }
                for (var i = 0; i < right.Count; i++)
                {
                    Console.Write("  ");
                }
                // display indented node title                  
                Console.WriteLine(child.Name);

                // add this node to the stack  
                right.Add(child.Rgt);
                //Console.WriteLine("Right: " + child.Rgt);
            }
        }
    }
}