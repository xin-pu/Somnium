using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Core;
using Somnium.Data;

namespace Somnium.Tests
{
    [TestClass]
    public class ModelTest
    {
        public string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas\smallDigits";

        [TestMethod]
        public void MatrixTest()
        {

        }

        [TestMethod]
        public void LoadInputLay()
        {
            var path = new FileInfo(Path.Combine(WorkFolder, "1_0.txt"));
            var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadLayerInput);
        }

        [TestMethod]
        public void LoadMultiInputLay()
        {
            var dir = new DirectoryInfo(WorkFolder);
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
            var path = new FileInfo(Path.Combine(WorkFolder, "1_0.txt"));
            var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadLayerInput);

            var fullConnectedLayer = new FullConnectedLayer(inputLayer.OutputDataSizeFormat, 6);
            var outputLayer = new OutputLayer(fullConnectedLayer.OutputDataSizeFormat, 3);


            fullConnectedLayer.DatasCheckIn(inputLayer.OutputDatas);
            outputLayer.DatasCheckIn(fullConnectedLayer.OutputData);

            outputLayer.Deviationed(inputLayer.ExpectVal, 0.1);
            fullConnectedLayer.Deviationed(outputLayer.ActivateNerveCells, 0.1);

        }


        [TestMethod]
        public void ExecuteAllLayOnce()
        {
            var gradient = 0.1;
            var dir = new DirectoryInfo(WorkFolder);
            var inputsLays = dir.GetFiles()
                .Select((path, index) =>
                {
                    var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadLayerInput);
                    inputLayer.LayerRowIndex = index;
                    inputLayer.LayerColumnIndex = 1;
                    return inputLayer;
                }).ToList();

            var inputLayFormat = inputsLays.First();
            var fullConnectedLayer = new FullConnectedLayer(inputLayFormat.OutputDataSizeFormat, 6);
            var outputLayer = new OutputLayer(fullConnectedLayer.OutputDataSizeFormat, 3);

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
            var iterations = 500;
            var gradient = 0.1;
            var dir = new DirectoryInfo(WorkFolder);
            var inputsLays = dir.GetFiles()
                .Select((path, index) =>
                {
                    var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadLayerInput);
                    inputLayer.LayerRowIndex = index;
                    inputLayer.LayerColumnIndex = 1;
                    return inputLayer;
                }).ToList();

            var inputLayFormat = inputsLays.First();
            var fullConnectedLayer = new FullConnectedLayer(inputLayFormat.OutputDataSizeFormat, 18);
            var outputLayer = new OutputLayer(fullConnectedLayer.OutputDataSizeFormat, 3);

            //Learning
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

            //Save Learning

            //Test Learning

            var C = new List<int>();
            inputsLays.ForEach(lay =>
            {
                fullConnectedLayer.DatasCheckIn(lay.OutputDatas);
                outputLayer.DatasCheckIn(fullConnectedLayer.OutputData);
                var likelihoodRatio = outputLayer.ActivatedOuput;
                C.Add(likelihoodRatio.IndexOf(likelihoodRatio.Max()) + 1);
            });

        }

    }
}
