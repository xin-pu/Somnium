using System;
using System.Collections.Generic;
using Somnium.Data;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Core;
using Somnium.Func;
using Somnium.Kernel;

namespace Console
{
    class Program
    {
        public static string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas\smallDigits";

        static void Main(string[] args)
        {
            TrainingDigits();
        }

        public static void ExecuteAllLayByIte()
        {
            var iterations = 10000;
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
            int count = 200;
            double gar = 0.2;
            //定义从文件获取数据流的方法，需要返回矩阵数据以及正确Label
            StreamData.GetStreamData = GetArrayStreamData;


            //读取数据层
            var dir = new DirectoryInfo(WorkFolder);
            var inputStreams = dir.GetFiles()
                .Where(path => path.Extension == ".txt")
                .Select((path, index) =>
                {
                    var inputLayer = StreamData.CreateStreamData(path.FullName);
                    return inputLayer;
                }).ToList();
            //映射标记结果
            var map = new LabelMap(inputStreams.Select(a => a.ExpectedLabel));
            inputStreams.ForEach(a => a.ExpectedOut = map.GetCorrectResult(a.ExpectedLabel));


            var dataShape = StreamData.FilterDataShape(inputStreams);

            StreamData.GetCost = Cost.GetVariance;
            StreamData.GetEstimateLabel = map.GetLabel;


            //创建神经网络层
            var layerStream = new StreamLayer();
            layerStream.AddInputLayer(new LayerInput(dataShape));
            layerStream.AddFullConnectedLayer(5);
            layerStream.AddOutputLayer(map.Count);

            for (int i = 0; i < count; i++)
            {
                //以神经网络层更新数据层
                inputStreams.AsParallel().ForAll(singleStream => singleStream.ActivateLayerNet(layerStream));

                var correct = inputStreams.Count(a => a.IsMeetExpect) * 100.0 / inputStreams.Count;
                System.Console.WriteLine($"{correct}%");
                if (correct > 80)
                    break;

                //反向传播误差
                inputStreams.AsParallel().ForAll(singleStream => singleStream.ErrorBackPropagation(layerStream, gar));

                //从数据层收集误差并更新神经网络层的神经元
                layerStream.UpdateWeight(inputStreams);

            }

        }

        public static StreamData GetArrayStreamData(string path)
        {
            using (var streamRead = new StreamReader(path))
            {
                var allData = new List<double>();
                var allLine = streamRead.ReadToEnd();
                var lines = allLine.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).ToList();
                lines.ForEach(line =>
                {
                    allData.AddRange(line.ToCharArray().Select(a => double.Parse(a.ToString())));
                });

                var matrix = new DenseMatrix(allData.Count, 1);
                matrix.SetColumn(0, allData.ToArray());
                var actual = new FileInfo(path).Name.Split('_').First();
                return new StreamData
                {
                    InputDataMatrix = matrix,
                    ExpectedLabel = actual
                };
            }
        }

    }
}
