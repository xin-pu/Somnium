using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Core;
using Somnium.Data;

namespace Somnium.Tests.ModelTest
{
    [TestClass]
    public class ModelTestSmallDigits
    {
        public string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas\smallDigits";



        [TestMethod]
        public void LoadInputLay()
        {
            var path = new FileInfo(Path.Combine(WorkFolder, "1_0.txt"));
            var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadDigitsAsInputLayer);
            Assert.AreEqual("1", inputLayer.Label);
        }

        [TestMethod]
        public void LoadMultiInputLay()
        {
            var dir = new DirectoryInfo(WorkFolder);
            var inputsLays = dir.GetFiles()
                .Select((path, index) =>
                {
                    var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadDigitsAsInputLayer);
                    inputLayer.LayerRowIndex = index;
                    inputLayer.LayerColumnIndex = 1;
                    return inputLayer;
                }).ToList();
            Assert.AreEqual(3, inputsLays.Select(a => a.Label).Distinct().Count());
        }


        [TestMethod]
        public void LoadOneInputDataAndActivate()
        {
            var path = new FileInfo(Path.Combine(WorkFolder, "1_0.txt"));
            var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadDigitsAsInputLayer);
            inputLayer.ExpectVal = new double[] {1, 0, 0};

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

            //Load Input Datas to Layers
            var inputsLays = new DirectoryInfo(WorkFolder).GetFiles()
                .Select((path, index) =>
                {
                    var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadDigitsAsInputLayer);
                    return inputLayer;
                }).ToList();

            //Create Map And Update ExpectVal
            var map = new LabelMap(inputsLays.Select(a => a.Label));
            inputsLays.ForEach(a => a.ExpectVal = map.GetCorrectResult(a.Label));

            var inputLayFormat = inputsLays.First();
            var fullConnectedLayer = new FullConnectedLayer(inputLayFormat.OutputDataSizeFormat, 6);
            var outputLayer = new OutputLayer(fullConnectedLayer.OutputDataSizeFormat, map.Count);

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
            var iterations = 1000;
            var gradient = 0.1;

            var inputsLays = new DirectoryInfo(WorkFolder).GetFiles()
                .Select((path, index) =>
                {
                    var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadDigitsAsInputLayer);
                    return inputLayer;
                }).ToList();

            var map = new LabelMap(inputsLays.Select(a => a.Label));
            inputsLays.ForEach(a => a.ExpectVal = map.GetCorrectResult(a.Label));

            var inputLayFormat = inputsLays.First();
            var fullConnectedLayer = new FullConnectedLayer(inputLayFormat.OutputDataSizeFormat, 18);
            var outputLayer = new OutputLayer(fullConnectedLayer.OutputDataSizeFormat, map.Count);

            //Learning
            Enumerable.Range(0, iterations).ToList().ForEach(i =>
            {
                inputsLays.ForEach(lay =>
                {
                    //Check in Datas
                    fullConnectedLayer.DatasCheckIn(lay.OutputDatas);
                    outputLayer.DatasCheckIn(fullConnectedLayer.OutputData);

                    //Cal Deviation Update Delta Weight and Bias
                    outputLayer.Deviationed(lay.ExpectVal, gradient);
                    fullConnectedLayer.Deviationed(outputLayer.ActivateNerveCells, gradient);
                });

                //Update Weight and Bias
                outputLayer.UpdateWeight();
                fullConnectedLayer.UpdateWeight();
            });

            //Save Learning

            //Test Learning
            var outputLabels = new List<string>();
            inputsLays.ForEach(lay =>
            {
                fullConnectedLayer.DatasCheckIn(lay.OutputDatas);
                outputLayer.DatasCheckIn(fullConnectedLayer.OutputData);
                var likelihoodRatio = outputLayer.GetLikelihoodRatio();
                var likeIndex = likelihoodRatio.IndexOf(likelihoodRatio.Max());
                outputLabels.Add(map.GetLabel(likeIndex));
            });

        }




    }
}
