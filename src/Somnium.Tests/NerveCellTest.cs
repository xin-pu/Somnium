
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Core;
using Somnium.Func;

namespace Somnium.Tests
{
    [TestClass]
    public class NerveCellTest
    {
        [TestMethod]
        public void NerveCellTestTest()
        {
            var d = new ActivateNerveCell(new DataSize {ColumnCount = 5, RowCount = 1})
            {
                ActivateFuc = Activate.Sigmoid
            };
            var inputData = DenseMatrix.Create(1, 5, 1);
            var activated = d.GetActivated(inputData);
            Assert.AreEqual(0.05, activated, 1);
        }

        
    }
}
