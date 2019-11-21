using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Core;
using Somnium.Core.Double;
using Somnium.Datas;

namespace Somnium.Tests
{
    [TestClass]
    public class ModelTest
    {
        [TestMethod]
        public void MatrixTest()
        {
            var res=ImageLoad.ReadLayerInput(@"E:\Document Code\Code Pensonal\Somnium\datas\trainingDigits\0_0.txt");
            Assert.AreEqual(res.Name, "0_0.txt");
        }

        [TestMethod]
        public void LoadMultiInputLay()
        {
            var dir = new DirectoryInfo(@"E:\Document Code\Code Pensonal\Somnium\datas\trainingDigits");
            var inputsLay = dir.GetFiles()
                .Select(path => ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadLayerInput));

        
        }
    }
}
