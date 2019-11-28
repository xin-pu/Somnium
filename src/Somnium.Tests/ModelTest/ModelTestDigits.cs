using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Core;
using Somnium.Data;

namespace Somnium.Tests.ModelTest
{
    [TestClass]
    public class ModelTestDigits
    {
        public string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas\trainingDigits";

        [TestMethod]
        public void ExecuteAllLayByIte()
        {
            var iterations = 1;
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
            var fullConnectedLayer2 = new FullConnectedLayer(inputLayFormat.OutputDataSizeFormat, 80);
            var outputLayer = new OutputLayer(fullConnectedLayer2.OutputDataSizeFormat, map.Count);

            //Learning
            Enumerable.Range(0, iterations).ToList().ForEach(i =>
            {
                inputsLays.ForEach(lay =>
                {
                    //Check in Datas
                    fullConnectedLayer2.DatasCheckIn(lay.OutputDatas);
                    outputLayer.DatasCheckIn(fullConnectedLayer2.OutputData);

                    //Cal Deviation Update Delta Weight and Bias
                    outputLayer.Deviationed(lay.ExpectVal, gradient);
                    fullConnectedLayer2.Deviationed(outputLayer.ActivateNerveCells, gradient);
                });

                //Update Weight and Bias
                outputLayer.UpdateWeight();
                fullConnectedLayer2.UpdateWeight();
            });

            //Save Learning

            //Test Learning
            var outputLabels = new List<string>();
            inputsLays.ForEach(lay =>
            {
                fullConnectedLayer2.DatasCheckIn(lay.OutputDatas);
                outputLayer.DatasCheckIn(fullConnectedLayer2.OutputData);
                var likelihoodRatio = outputLayer.GetLikelihoodRatio();
                var likeIndex = likelihoodRatio.IndexOf(likelihoodRatio.Max());
                outputLabels.Add(map.GetLabel(likeIndex));
            });

        }




    }
}

