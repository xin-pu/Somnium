using System;
using System.Collections.Generic;
using Somnium.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Func;
using Somnium.Kernel;
using Somnium.Train;

namespace Console
{
    class Program
    {
        //public static string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas\newTest";
        public static string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas\smallDigits";

        static void Main(string[] args)
        {
           TrainingSmallDigitsWithExtendMethod();
        }


        public static void TrainingDigits()
        {
            int count = 20000;
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
            var layerStream = new StreamLayer(dataShape, map.Count, new[] { 6 },0.01);


            for (int i = 0; i < count; i++)
            {
                //以神经网络层更新数据层
                inputStreams.AsParallel().ForAll(singleStream => singleStream.ActivateLayerNet(layerStream));

                var correct = inputStreams.Count(a => a.IsMeetExpect) * 100.0 / inputStreams.Count;
                System.Console.WriteLine($"{correct}%");
            

                //反向传播误差
                inputStreams.AsParallel().ForAll(singleStream => singleStream.ErrorBackPropagation(layerStream));

                //从数据层收集误差并更新神经网络层的神经元
                layerStream.UpdateWeight(inputStreams);

            }

            layerStream.Serializer("D:\\1.xml");
            
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


        public static void TrainingSmallDigitsWithExtendMethod()
        {
            var train = new BasicDeepLearning();

            // 加载数据集
            var workFolder = @"D:\Document Code\Code Somnium\Somnium\datas\smallDigits";
            var trainDataManager = new TrainDataManager(workFolder);

            // 创建神经网络层
            var layerStream = new StreamLayer(trainDataManager.DataShapeFormat, trainDataManager.OutTypeCount,
                new[] {6},
                train.LearningRate);

            //// 创建训练
            train.ExecuteTrain(layerStream, trainDataManager);
        }



    }
}
