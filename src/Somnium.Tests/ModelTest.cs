using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Core;
using Somnium.Datas;

namespace Somnium.Tests
{
    [TestClass]
    public class ModelTest
    {
        [TestMethod]
        public void MatrixTest()
        {

        }

        [TestMethod]
        public void LoadInputLay()
        {
            var path = new DirectoryInfo(@"E:\Document Code\Code Pensonal\Somnium\datas\trainingDigits\0_0.txt");
            var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadLayerInput);
        }

        [TestMethod]
        public void LoadMultiInputLay()
        {
            var dir = new DirectoryInfo(@"E:\Document Code\Code Pensonal\Somnium\datas\trainingDigits");
            var inputsLays = dir.GetFiles()
                .Select((path, index) =>
                {
                    var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadLayerInput);
                    inputLayer.LayerRowIndex = index;
                    inputLayer.LayerColumnIndex = 1;
                    return inputLayer;
                }).ToList();

        }


        [TestMethod]
        public void LoadBasicDeepLeaning()
        {
            var path = new DirectoryInfo(@"E:\Document Code\Code Pensonal\Somnium\datas\trainingDigits\0_0.txt");
            var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadLayerInput);

            var fullConnectedLayer = new FullConnectedLayer(inputLayer.OutputDataSizeFormat, 20);
            var outputLayer = new OutputLayer(fullConnectedLayer.OutputDataSizeFormat, 10);


            fullConnectedLayer.DatasCheckIn(inputLayer.OutputDatas);
            outputLayer.DatasCheckIn(fullConnectedLayer.OutputData);

            outputLayer.Deviationed(inputLayer.ExpectVal, 0.1);
            fullConnectedLayer.Deviationed(outputLayer.ActivateNerveCells, 0.1);

        }


        [TestMethod]
        public void ExecuteAllLayOnce()
        {
            var gradient = 0.1;
            var dir = new DirectoryInfo(@"E:\Document Code\Code Pensonal\Somnium\datas\trainingDigits");
            var inputsLays = dir.GetFiles()
                .Select((path, index) =>
                {
                    var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadLayerInput);
                    inputLayer.LayerRowIndex = index;
                    inputLayer.LayerColumnIndex = 1;
                    return inputLayer;
                }).ToList();

            var inputLayFormat = inputsLays.First();
            var fullConnectedLayer = new FullConnectedLayer(inputLayFormat.OutputDataSizeFormat, 20);
            var outputLayer = new OutputLayer(fullConnectedLayer.OutputDataSizeFormat, 10);

            inputsLays.ForEach(lay =>
            {
                fullConnectedLayer.DatasCheckIn(lay.OutputDatas);
                outputLayer.DatasCheckIn(fullConnectedLayer.OutputData);

                outputLayer.Deviationed(lay.ExpectVal, gradient);
                fullConnectedLayer.Deviationed(outputLayer.ActivateNerveCells, gradient);

            });

            outputLayer.UpdateWeight();
            fullConnectedLayer.UpdateWeight();

        }

        [TestMethod]
        public void ExecuteAllLayByIte()
        {
            var iterations = 50;
            var gradient = 0.1;
            var dir = new DirectoryInfo(@"E:\Document Code\Code Pensonal\Somnium\datas\trainingDigits");
            var inputsLays = dir.GetFiles()
                .Select((path, index) =>
                {
                    var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadLayerInput);
                    inputLayer.LayerRowIndex = index;
                    inputLayer.LayerColumnIndex = 1;
                    return inputLayer;
                }).ToList();

            var inputLayFormat = inputsLays.First();
            var fullConnectedLayer = new FullConnectedLayer(inputLayFormat.OutputDataSizeFormat, 20);
            var outputLayer = new OutputLayer(fullConnectedLayer.OutputDataSizeFormat, 10);

            Enumerable.Range(0, iterations).ToList().ForEach(i =>
            {
                inputsLays.ForEach(lay =>
                {
                    fullConnectedLayer.DatasCheckIn(lay.OutputDatas);
                    outputLayer.DatasCheckIn(fullConnectedLayer.OutputData);

                    outputLayer.Deviationed(lay.ExpectVal, gradient);
                    fullConnectedLayer.Deviationed(outputLayer.ActivateNerveCells, gradient);

                });

                outputLayer.UpdateWeight();
                fullConnectedLayer.UpdateWeight();
            });

        }

    }
}
