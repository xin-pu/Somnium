using System;
using System.Collections.Generic;
using Somnium.Data;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Func;
using Somnium.Kernel;

namespace Console
{
    class Program
    {
        //public static string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas\newTest";
        public static string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas\test";

        static void Main(string[] args)
        {
            TrainingDigits();
        }

        
        public static void TrainingDigits()
        {
            int count = 5;
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
            layerStream.AddFullConnectedLayer(8);
            layerStream.AddOutputLayer(map.Count);

            //var secondlayer = (LayerFullConnected) layerStream.LayerQueue.ToArray()[1];
            //secondlayer.Perceptrons[0].Weight.SetColumn(0,new[]
            //{
            //    0.490,0.348,0.073,
            //    0.837,-0.071,-3.671,
            //    -0.536,-0.023,-1.717,
            //    -1.456,-0.556,0.852
            //});
            //secondlayer.Perceptrons[1].Weight.SetColumn(0, new[]
            //{
            //    0.442,-0.537,1.008,
            //    1.072,-0.733,0.823,
            //    -0.453,-0.014,-0.027,
            //    -0.427,1.876,-2.305
            //});
            //secondlayer.Perceptrons[2].Weight.SetColumn(0, new[]
            //{
            //    0.654,-1.389,1.246,
            //    0.057,-0.183,-0.743,
            //    -0.461,0.331,0.449,
            //    -1.296,1.569,-0.471
            //});
            //secondlayer.Perceptrons[0].Offset = -0.185;
            //secondlayer.Perceptrons[1].Offset = 0.526;
            //secondlayer.Perceptrons[2].Offset = -1.169;


            //var outputlayer = (LayerOutput)layerStream.LayerQueue.ToArray()[2];
            //outputlayer.Perceptrons[0].Weight.SetColumn(0, new[]
            //{
            //    0.388,0.803,0.029
            //});
            //outputlayer.Perceptrons[1].Weight.SetColumn(0, new[]
            //{
            //    0.025,-0.790,1.553
            //});
            //outputlayer.Perceptrons[0].Offset =-1.438;
            //outputlayer.Perceptrons[1].Offset = -1.379;
            

            for (int i = 0; i < count; i++)
            {
                //以神经网络层更新数据层
                inputStreams.AsParallel().ForAll(singleStream => singleStream.ActivateLayerNet(layerStream));

                var correct = inputStreams.Count(a => a.IsMeetExpect) * 100.0 / inputStreams.Count;
                System.Console.WriteLine($"{correct}%");
            

                //反向传播误差
                inputStreams.AsParallel().ForAll(singleStream => singleStream.ErrorBackPropagation(layerStream, gar));

                //从数据层收集误差并更新神经网络层的神经元
                layerStream.UpdateWeight(inputStreams);

            }

            layerStream.SaveLayer("D:\\1.xml");
            
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
