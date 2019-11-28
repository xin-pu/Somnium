using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Data;
using Somnium.Model;

namespace Somnium.Tests.ModelTest
{
    [TestClass]
    public class SingleFclTest
    {

        public string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas\smallDigits";


        [TestMethod]
        public void TestSingleFclMode()
        {
            var inputsLays = new DirectoryInfo(WorkFolder).GetFiles()
                .Select((path, index) =>
                {
                    var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadDigitsAsInputLayer);
                    return inputLayer;
                }).ToList();

            var train = new SingleFclTrain(100,8,0.1)
            {
                ReadLayerInput = ImageLoad.ReadDigitsAsInputLayer
            };
            train.CreateTrainModel(WorkFolder);
         
        }
    }
}
