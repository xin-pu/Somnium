using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Data;
using Somnium.Kernel;

namespace Somnium.Tests.LayerTest
{
    [TestClass]
    public class OutputLayerTest
    {
        public string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas\smallDigits";


        [TestMethod]
        public void TestInitial()
        {
            var outputLayer = new LayerOutput(new DataShape(5, 1), 1);
            Assert.AreNotEqual(outputLayer, null);
        }


        [TestMethod]
        public void TestSerializer()
        {
            var path = "D:\\3.xml";
            var outputLayer = new LayerOutput(new DataShape(5, 1), 1);
            outputLayer.Serializer(path);
            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void TestDeserialize()
        {
            var path = "D:\\3.xml";
            var layOutput = (LayerOutput) LayerOutput.Deserialize(path);
            Assert.AreEqual(layOutput.Perceptrons[0].WeightArray.Length, 5);
        }
    }
}
