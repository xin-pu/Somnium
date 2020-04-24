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
        public static string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas\trainingDigits";

        static void Main(string[] args)
        {
            TrainingDigits();
        }

        
        public static void TrainingDigits()
        {
            int count = 20000;
            double gar = 0.1;
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
            layerStream.AddFullConnectedLayer(20);
            layerStream.AddOutputLayer(map.Count);

            
            
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
