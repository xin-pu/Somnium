using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using Somnium.PreTreatment;

namespace Somnium.Tests.OpenCVTest
{
    [TestClass]
    public class OpenCvTest
    {
        [TestMethod]
        public void CreateMatrix()
        {
            var mat = OpenCv.GetMat(4, 4, new double[] {1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4}, MatType.CV_8S);
        }

    }
}
