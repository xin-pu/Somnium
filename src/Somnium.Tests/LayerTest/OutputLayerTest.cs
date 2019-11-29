using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Core;
using Somnium.Data;

namespace Somnium.Tests.LayerTest
{
    [TestClass]
    public class OutputLayerTest
    {
        public string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas\smallDigits";


        [TestMethod]
        public void TestInitial()
        {
            var path = new FileInfo(Path.Combine(WorkFolder, "1_0.txt"));
            var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadDigitsAsInputLayer);
            var fullConnectedLayer = new FullConnectedLayer(inputLayer.OutputDataSizeFormat, 6);
            var outputLayer = new OutputLayer(fullConnectedLayer.OutputDataSizeFormat, 3);
        }


        [TestMethod]
        public void TestSave()
        {
            var path = new FileInfo(Path.Combine(WorkFolder, "1_0.txt"));
            var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadDigitsAsInputLayer);
            var fullConnectedLayer = new FullConnectedLayer(inputLayer.OutputDataSizeFormat, 6);
            var outputLayer = new OutputLayer(fullConnectedLayer.OutputDataSizeFormat, 3);
            outputLayer.Save(Path.Combine(WorkFolder, "output.xml"));
        }
    }
}
