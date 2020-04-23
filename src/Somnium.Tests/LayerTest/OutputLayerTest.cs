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
            var outputLayer=new LayerOutput(new DataShape(5,1,1), 1);
            outputLayer.Serializer("D:\\test.xml");
        }


        [TestMethod]
        public void TestSave()
        {
          
        }
    }
}
