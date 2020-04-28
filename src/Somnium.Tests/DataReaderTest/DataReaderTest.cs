using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Data;

namespace Somnium.Tests.DataReaderTest
{
    [TestClass]
    public class DataReaderTest
    {
        [TestMethod]
        public void GetClass()
        {
            DataReader.GetDataReaders();
        }
    }
}
