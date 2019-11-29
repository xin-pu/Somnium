using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Data;

namespace Somnium.Tests.LayerTest
{
    [TestClass]
    public class InputLayerTest
    {

        public string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas\smallDigits";


        [TestMethod]
        public void TestInitial()
        {
            var path = new FileInfo(Path.Combine(WorkFolder, "1_0.txt"));
            var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadDigitsAsInputLayer);
            Assert.AreEqual("1", inputLayer.Label);
        }

        [TestMethod]
        public void TestSave()
        {
            var path = new FileInfo(Path.Combine(WorkFolder, "1_0.txt"));
            var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadDigitsAsInputLayer);
            inputLayer.Save(Path.Combine(WorkFolder, "1_0.xml"));
        }

        [TestMethod]
        public void LoadMultiInputLay()
        {
            var dir = new DirectoryInfo(WorkFolder);
            var inputsLays = dir.GetFiles()
                .Where(path => path.Extension == ".txt")
                .Select((path, index) =>
                {
                    var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadDigitsAsInputLayer);
                    inputLayer.LayerRowIndex = index;
                    inputLayer.LayerColumnIndex = 1;
                    return inputLayer;
                }).ToList();
            Assert.AreEqual(3, inputsLays.Select(a => a.Label).Distinct().Count());
        }
    }
}
