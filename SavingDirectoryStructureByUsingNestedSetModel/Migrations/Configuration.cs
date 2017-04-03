namespace SavingDirectoryStructureByUsingNestedSetModel.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SavingDirectoryStructureByUsingNestedSetModel.Models.TreeContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "SavingDirectoryStructureByUsingNestedSetModel.Models.TreeContext";
        }

        protected override void Seed(Models.TreeContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StoredProcedures");
            path = path.Replace(@"\bin\Debug\", @"\");
            
            if (path != null)
            {
                var sortedFiles = new DirectoryInfo(path).GetFiles().ToList();
                if (sortedFiles.Any())
                {
                    //clear all functions
                    var query = @"declare @procName varchar(500)
                                    declare cur cursor 

                                    for select [name] from sys.objects where type = 'p'
                                    open cur
                                    fetch next from cur into @procName
                                    while @@fetch_status = 0
                                    begin
                                        exec('drop procedure [' + @procName + ']')
                                        fetch next from cur into @procName
                                    end
                                    close cur
                                    deallocate cur";

                    context.Database.ExecuteSqlCommand(query);
                    foreach (var file in sortedFiles)
                    {
                        var sqlCommand = File.ReadAllText(file.FullName);
                        context.Database.ExecuteSqlCommand(sqlCommand);
                    }

                }
            }
        }
    }
}
