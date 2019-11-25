using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Datas;

namespace Somnium.Tests
{
    [TestClass]
    public class ModelTest
    {
        [TestMethod]
        public void MatrixTest()
        {
            
        }

        [TestMethod]
        public void LoadInputLay()
        {
            var path = new DirectoryInfo(@"E:\Document Code\Code Pensonal\Somnium\datas\trainingDigits\0_0.txt");
            var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadLayerInput);
        }

        [TestMethod]
        public void LoadMultiInputLay()
        {
            var dir = new DirectoryInfo(@"E:\Document Code\Code Pensonal\Somnium\datas\trainingDigits");
            var inputsLays = dir.GetFiles()
                .Select((path, index) =>
                {
                    var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadLayerInput);
                    inputLayer.LayerRowIndex = index;
                    inputLayer.LayerColumnIndex = 1;
                    return inputLayer;
                }).ToList();
            
        }


        [TestMethod]
        public void LoadBasicDeepLeaning()
        {


        }


    }
}
