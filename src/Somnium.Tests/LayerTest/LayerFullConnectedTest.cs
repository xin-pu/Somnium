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
            var outputLayer = new LayerFullConnected(new DataShape(5,1), 1);
            outputLayer.Serializer("D:\\test.xml");
        }
    }
}
