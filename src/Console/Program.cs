using Somnium.Data;
using System.IO;
using System.Linq;
using Somnium.Core;

namespace Console
{
    class Program
    {
        public static string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas\trainingDigits";

        static void Main(string[] args)
        {
            TrainingDigits();
        }

        public static void ExecuteAllLayByIte()
        {
            var iterations =10000;
            var gradient = 0.1;
            var fullConnectCount = 18;

            var inputsLays = new DirectoryInfo(WorkFolder).GetFiles()
                .Select((path, index) =>
                {
                    var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadDigitsAsInputLayer);
                    return inputLayer;
                }).ToList();

            var map = new LabelMap(inputsLays.Select(a => a.Label));
            inputsLays.ForEach(a => a.ExpectVal = map.GetCorrectResult(a.Label));

            var inputLayFormat = inputsLays.First();
            var fullConnectedLayer = new FullConnectedLayer(inputLayFormat.OutputDataSizeFormat, fullConnectCount);
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
                    outputLayer.Deviated(lay.ExpectVal, gradient);
                    fullConnectedLayer.Deviationed(outputLayer.ActivateNerveCells, gradient);
                });

                //Update Weight and Bias
                outputLayer.UpdateWeight();
                fullConnectedLayer.UpdateWeight();

                //Test Learning
                inputsLays.ForEach(lay =>
                {
                    fullConnectedLayer.DatasCheckIn(lay.OutputDatas);
                    outputLayer.DatasCheckIn(fullConnectedLayer.OutputData);
                    var likelihoodRatio = outputLayer.GetLikelihoodRatio();
                    var likeIndex = likelihoodRatio.IndexOf(likelihoodRatio.Max());
                    lay.LabelEstimate = map.GetLabel(likeIndex);
                });
                var cor = 100 * inputsLays.Count(a => a.LabelEstimate == a.Label) / (double) inputsLays.Count;
                System.Console.WriteLine($"Count{i} Accuracy {cor}%");
            });
        }

        public static void TrainingDigits()
        {
            var iterations = 10000;
            var gradient = 0.1;
            var fullConnectCount = 80;

            var inputsLays = new DirectoryInfo(WorkFolder).GetFiles()
                .Select((path, index) =>
                {
                    var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadDigitsAsInputLayer);
                    return inputLayer;
                }).ToList();

            var map = new LabelMap(inputsLays.Select(a => a.Label));
            inputsLays.ForEach(a => a.ExpectVal = map.GetCorrectResult(a.Label));

            var inputLayFormat = inputsLays.First();
            var fullConnectedLayer2 = new FullConnectedLayer(inputLayFormat.OutputDataSizeFormat, fullConnectCount);
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
                    outputLayer.Deviated(lay.ExpectVal, gradient);
                    fullConnectedLayer2.Deviationed(outputLayer.ActivateNerveCells, gradient);
                });

                //Update Weight and Bias
                outputLayer.UpdateWeight();
                fullConnectedLayer2.UpdateWeight();

                //Test Learning
                inputsLays.ForEach(lay =>
                {
                    fullConnectedLayer2.DatasCheckIn(lay.OutputDatas);
                    outputLayer.DatasCheckIn(fullConnectedLayer2.OutputData);
                    var likelihoodRatio = outputLayer.GetLikelihoodRatio();
                    var likeIndex = likelihoodRatio.IndexOf(likelihoodRatio.Max());
                    lay.LabelEstimate = map.GetLabel(likeIndex);
                });
                var cor = 100 * inputsLays.Count(a => a.LabelEstimate == a.Label) / (double)inputsLays.Count;
                System.Console.WriteLine($"Count{i} Accuracy {cor}%");
            });

        }


    }
}
