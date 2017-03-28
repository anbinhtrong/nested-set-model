using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeTraversal.Database;

namespace TreeTraversal
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayTree("Food");
            Console.WriteLine("*********************");
            DisplayParent("Cherry");
        }

        static void DisplayTree(string root)
        {
            var db = new Entities();

            // retrieve the left and right value of the $root node  
            var query = $"SELECT * FROM tree WHERE title='{root}';";
            var result1 = db.Database.SqlQuery<tree>(query).ToList();
            var right = new List<int>();

            // now, retrieve all descendants of the $root node  

            query = "SELECT * FROM tree " +  $"WHERE [Left] BETWEEN {result1[0].Left} AND {result1[0].Right} ORDER BY [Left] ASC;";
            var children = db.Database.SqlQuery<tree>(query).ToList();
            // display each row  
            foreach(tree child in children) {
                // only check stack if there is one  
                if (right.Count > 0)
                {
                    // check if we should remove a node from the stack  
                    while (right[right.Count - 1] < child.Right) {
                        right.RemoveAt(right.Count - 1);
                    }
                }
                for(var i = 0; i < right.Count; i++)
                {
                    Console.Write("  ");
                }
                // display indented node title                  
                Console.WriteLine(child.Title);  
  
                // add this node to the stack  
                right.Add(child.Right);
            }
        }

        static void DisplayParent(string childNode)
        {
            var db = new Entities();
            var query = $"SELECT * FROM tree WHERE title='{childNode}';";
            var result1 = db.Database.SqlQuery<tree>(query).FirstOrDefault();
            query = $"SELECT * FROM tree WHERE[Left] < {result1.Left} AND[right] > {result1.Right} ORDER BY[Left] ASC; ";
            var parents = db.Database.SqlQuery<tree>(query).ToList();
            foreach(tree parent in parents)
            {
                Console.WriteLine(parent.Title);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        static int RebuildTree(string parent, int left)
        {
            var db = new Entities();
            int right = left + 1;
            // get all children of this node   
            var query = ("SELECT title FROM tree " + $"WHERE parent='{parent}';");
            var children = db.Database.SqlQuery<tree>(query).ToList();

            foreach(var child in children)
            {
                right = RebuildTree(child.Title, right);
            }

            //// we've got the left value, and now that we've processed   
            //// the children of this node we also know the right value   

            db.Database.ExecuteSqlCommand($"UPDATE tree SET [Left]='{left} ', [right]='{right}' WHERE title='{parent}'");

            //// return the right value of this node + 1   
            return right + 1;
        }

        public static void AddNewNode(string newNodeName, int newLeft)
        {
            var db = new Entities();
            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"UPDATE tree SET [right]=[right]+2 WHERE [right]>5;");
            queryBuilder.Append("UPDATE tree SET [left] = lft + 2 WHERE lft > 5;");
            queryBuilder.Append("INSERT INTO tree SET lft=6, [right]=7, title='Strawberry';");
        }
    }
}
