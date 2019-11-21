using System;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Core.Double;
using Somnium.Func;

namespace Somnium.Tests
{
    [TestClass]
    public class NerveCellTest
    {
        [TestMethod]
        public void NerveCellTestTest()
        {
            var d = new NerveCellActivate(1,5)
            {
                ActivateFuc = Activate.sigmoid
            };
            var inputData = DenseMatrix.Create(1, 5, 1);
            var layerIn = new LayerInput(inputData, 1);
            var layout = d.ActivateNerveCell(layerIn);
            Assert.AreEqual(0, layout, 1E-1);
        }

        [TestMethod]
        public void MatrixTest()
        {
           

        }


    }
}
