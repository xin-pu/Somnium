using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Core;
using Somnium.Func;
using Somnium.Train;

namespace Console
{
    class Program
    {
        

        static void Main()
        {
            TrainingSmallDigitsWithExtendMethod();
        }


        public static void TrainingSmallDigitsWithExtendMethod()
        {
            // 初始化训练参数
            var trainParameters = new TrainParameters
            {
                LearningRate = 0.1,
                TrainCountLimit = 1000,
                CostType = CostType.Basic,
                LikeliHoodType = LikeliHoodType.SoftMax
            };

            //// 创建训练
            var train = new DeepLeaningModel(trainParameters);

            // 根据数据目录,以及读取文件流的方法，加载数据集
            const string workFolder = @"D:\Document Code\Code Somnium\Somnium\datas\trainingDigits";
            var trainDataManager = new TrainDataManager(workFolder, GetStreamDataFromFolder);
            trainDataManager.Binding(trainParameters);
            
            // 创建神经网络层
            var layNet = new LayerNetManager(trainDataManager, trainParameters,
                new LayNetParameter
                {
                    FullConnectLayer = new[] {32, 16}
                });

            // 执行训练
            train.ExecuteTrain(layNet, trainDataManager);
        }

        public static StreamData GetStreamDataFromFolder(string path)
        {
            using (var streamRead = new StreamReader(path))
            {
                var allData = new List<double>();
                var allLine = streamRead.ReadToEnd();
                var lines = allLine.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
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
