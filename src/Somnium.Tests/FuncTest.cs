using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Func;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Somnium.Tests
{
    [TestClass]
    public class FuncTest
    {
        [TestMethod]
        public void ActivationTest()
        {
            var activations = new List<Func<double, double>> { Activate.sigmoid, Activate.tanh, Activate.max };
            var res = activations.Select(a => a(0)).ToArray();
            Assert.AreEqual(0.5, res[0], 1E-4);
            Assert.AreEqual(0, res[1], 1E-4);
            Assert.AreEqual(0, res[2], 1E-4);
        }
    }
}
