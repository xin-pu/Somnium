using System;
using System.IO;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Kernel;

namespace Somnium.Tests.LayerTest
{
    [TestClass]
    public class InputLayerTest
    {

        public string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas\smallDigits";


        [TestMethod]
        public void TestInitial()
        {
            var inputLayer = new LayerInput(new DataShape(4, 1));
            Assert.AreNotEqual(inputLayer, null);
        }



        [TestMethod]
        public void TestActivated()
        {
            var inputLayer = new LayerInput(new DataShape(4, 1));
            var (item1, item2) = inputLayer.Activated(new DenseMatrix(4, 1));
            Assert.AreEqual(item1, item2);
        }


        [TestMethod]
        public void TestSerializer()
        {
            var path = "D:\\1.xml";
            var inputLayer = new LayerInput(new DataShape(4, 1));
            inputLayer.Serializer(path);
            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void TestDeserialize()
        {
            var path = "D:\\1.xml";
            var layInput = (LayerInput) LayerInput.Deserialize(path);
            Assert.AreEqual(layInput.ShapeIn.Rows, 4);
        }


    }
}
