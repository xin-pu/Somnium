using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Func;

namespace Somnium.Tests
{
    [TestClass]
    public class FuncTest
    {
        [TestMethod]
        public void ActivationTest()
        {

            var res0 = Activate.Sigmoid(0);
            var res1 = Activate.Max(0);
            var res2 = Activate.Tanh(0);
            Assert.AreEqual(0.5, res0, 1E-4);
            Assert.AreEqual(0, res1, 1E-4);
            Assert.AreEqual(0, res2, 1E-4);
        }
    }
}
