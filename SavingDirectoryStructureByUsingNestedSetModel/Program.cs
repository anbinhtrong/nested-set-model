using SavingDirectoryStructureByUsingNestedSetModel.Models;
using SavingDirectoryStructureByUsingNestedSetModel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SavingDirectoryStructureByUsingNestedSetModel
{
    class Program
    {
        static void Main(string[] args)
        {
            var treeMapService = new TreeMapService();
            //var treeContentService = new TreeContentService();

            //treeMapService.InsertNewNode(-1, "Home", FileTypeEnum.Directory);
            ////treeMapService.DisplayRootTree();

            //var homeNode = treeMapService.GetByName("Home").First();

            //treeMapService.InsertNewNode(homeNode.Id, "HTML", FileTypeEnum.Directory);
            //treeMapService.InsertNewNode(homeNode.Id, "JAVASCRIPT", FileTypeEnum.Directory);
            //treeMapService.InsertNewNode(homeNode.Id, "PHP", FileTypeEnum.Directory);
            //treeMapService.InsertNewNode(homeNode.Id, "NODE JS", FileTypeEnum.Directory);

            //var javascript = treeMapService.GetByName("JAVASCRIPT").First();

            //treeMapService.InsertNewNode(javascript.Id, "Angular", FileTypeEnum.Directory);
            //treeMapService.InsertNewNode(javascript.Id, "BackBone", FileTypeEnum.Directory);
            //treeMapService.InsertNewNode(javascript.Id, "JQuery", FileTypeEnum.Directory);

            //var php = treeMapService.GetByName("PHP").First();
            //treeMapService.InsertNewNode(php.Id, "Zend", FileTypeEnum.File);

            //var nodeJs = treeMapService.GetByName("NODE JS").First();
            //treeMapService.InsertNewNode(nodeJs.Id, "Cloud", FileTypeEnum.File);
            //treeMapService.InsertNewNode(nodeJs.Id, "Debug", FileTypeEnum.File);
            //generateData();
            treeMapService.DisplayRootTree();

            var javascriptNode = treeMapService.GetByName("JAVASCRIPT").FirstOrDefault();
            var phpNode = treeMapService.GetByName("PHP").FirstOrDefault();
            //treeMapService.MoveToRightSide(javascriptNode, phpNode);
            treeMapService.DisplayRootTree();
        }

        public static void generateData()
        {
            var treeMapService = new TreeMapService();
            treeMapService.InsertNewNode(-1, "Home", FileTypeEnum.Directory);
            //treeMapService.DisplayRootTree();

            var homeNode = treeMapService.GetByName("Home").First();

            treeMapService.InsertNewNode(homeNode.Id, "HTML", FileTypeEnum.Directory);
            treeMapService.InsertNewNode(homeNode.Id, "JAVASCRIPT", FileTypeEnum.Directory);
            treeMapService.InsertNewNode(homeNode.Id, "PHP", FileTypeEnum.Directory);
            treeMapService.InsertNewNode(homeNode.Id, "NODE JS", FileTypeEnum.Directory);

            var javascript = treeMapService.GetByName("JAVASCRIPT").First();

            treeMapService.InsertNewNode(javascript.Id, "Angular", FileTypeEnum.Directory);
            treeMapService.InsertNewNode(javascript.Id, "BackBone", FileTypeEnum.Directory);
            treeMapService.InsertNewNode(javascript.Id, "JQuery", FileTypeEnum.Directory);

            var php = treeMapService.GetByName("PHP").First();
            treeMapService.InsertNewNode(php.Id, "Zend", FileTypeEnum.File);

            var nodeJs = treeMapService.GetByName("NODE JS").First();
            treeMapService.InsertNewNode(nodeJs.Id, "Cloud", FileTypeEnum.File);
            treeMapService.InsertNewNode(nodeJs.Id, "Debug", FileTypeEnum.File);
        }
    }
}
