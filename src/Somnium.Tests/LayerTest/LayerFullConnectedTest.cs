using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Kernel;

namespace Somnium.Tests.LayerTest
{
    [TestClass]
    public class LayerFullConnectedTest
    {
        [TestMethod]
        public void TestInitial()
        {
            var outputLayer = new LayerFullConnected(new DataShape(5, 1), 1);
            Assert.AreNotEqual(outputLayer, null);
        }


        [TestMethod]
        public void TestSerializer()
        {
            var path = "D:\\2.xml";
            var layerFullConnected = new LayerFullConnected(new DataShape(5, 1), 1);
            layerFullConnected.Serializer(path);
            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void TestDeserialize()
        {
            var path = "D:\\2.xml";
            var layerFullConnected = (LayerFullConnected)LayerFullConnected.Deserialize(path);
            Assert.AreEqual(layerFullConnected.Perceptrons[0].WeightArray.Length, 5);
        }
    }
}
