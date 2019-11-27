using Somnium.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Core;

namespace Console
{
    class Program
    {
        public static string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas\smallDigits";

        static void Main(string[] args)
        {
            ExecuteAllLayByIte();
        }

        public static void ExecuteAllLayByIte()
        {
            var iterations = 500;
            var gradient = 0.1;

            var inputsLays = new DirectoryInfo(WorkFolder).GetFiles()
                .Select((path, index) =>
                {
                    var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ReadLayerInput);
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
            System.Console.WriteLine(string.Join("\t", outputLabels));
        }


        #region
        private static InputLayer ReadLayerInput(string path)
        {
            using (var streamRead = new StreamReader(path))
            {
                var allLine = streamRead.ReadToEnd();
                var lines = allLine.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var matrix = DenseMatrix.Create(lines.Count, lines.First().Length,0);
                var rowIndex = 0;
                lines.ForEach(line =>
                {
                    var lineData = line.ToCharArray().Select(a => double.Parse(a.ToString()));
                    matrix.SetRow(rowIndex, lineData.ToArray());
                    rowIndex++;
                });
                var excepted = new FileInfo(path).Name.Split('_').First();
                var inputLayer = new InputLayer(
                    new DataSize { RowCount = matrix.RowCount, ColumnCount = matrix.ColumnCount });
                inputLayer.DatasCheckIn(matrix, excepted);
                return inputLayer;
            }
            
        }
        #endregion
    }
}
