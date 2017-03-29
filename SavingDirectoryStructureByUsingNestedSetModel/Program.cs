using SavingDirectoryStructureByUsingNestedSetModel.Models;
using SavingDirectoryStructureByUsingNestedSetModel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingDirectoryStructureByUsingNestedSetModel
{
    class Program
    {
        static void Main(string[] args)
        {
            var treeMapService = new TreeMapService();
            var treeContentService = new TreeContentService();

            treeMapService.InsertNewNode(-1, "Home", FileTypeEnum.Directory);
            //treeMapService.DisplayRootTree();

            var homeNode = treeMapService.GetByName("Home").First();

            treeMapService.InsertNewNode(homeNode.Id, ".NET", FileTypeEnum.Directory);
            treeMapService.InsertNewNode(homeNode.Id, "Java", FileTypeEnum.Directory);
            treeMapService.InsertNewNode(homeNode.Id, "Node", FileTypeEnum.Directory);

            var java = treeMapService.GetByName("Java").First();

            treeMapService.InsertNewNode(java.Id, "Spring", FileTypeEnum.Directory);
            treeMapService.InsertNewNode(java.Id, "Selenium", FileTypeEnum.Directory);
            treeMapService.InsertNewNode(java.Id, "FX", FileTypeEnum.Directory);
            treeMapService.InsertNewNode(java.Id, "selenium.jpg", FileTypeEnum.File);
            treeMapService.InsertNewNode(java.Id, "pikachu.jpg", FileTypeEnum.File);
            treeMapService.InsertNewNode(java.Id, "kabigon.jpg", FileTypeEnum.File);

            treeMapService.DisplayRootTree();
        }
    }
}
