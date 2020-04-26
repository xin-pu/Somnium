using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using OpenCvSharp;
using Somnium.Core;
using Somnium.Func;
using Somnium.Train;

namespace Console
{
    class Program
    {
        private static string workFolder = @"D:\Document Code\Code Somnium\Somnium\datas\trainingDigits";

        static void Main()
        {
            TrainingSmallDigitsWithExtendMethod();
        }


        public static void TrainingSmallDigitsWithExtendMethod()
        {
            // 初始化训练参数
            var trainParameters = new TrainParameters
            {
                LearningRate = 0.2,
                TrainCountLimit = 10000,
                CostType = CostType.Basic,
                LikeliHoodType = LikeliHoodType.SoftMax
            };

            //// 创建训练
            var train = new DeepLeaningModel(trainParameters);

            // 根据数据目录,以及读取文件流的方法，加载数据集
            const string workFolder = @"D:\Document Code\Code Somnium\Somnium\datas\resizeDigits";
            var trainDataManager = new TrainDataManager(workFolder, GetStreamDataFromResizeFolder);
            trainDataManager.Binding(trainParameters);

            // 创建神经网络层
            var layNet = new LayerNetManager(trainDataManager, trainParameters,
                new LayNetParameter
                {
                    FullConnectLayer = new[] {16}
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

        public static StreamData GetStreamDataFromResizeFolder(string path)
        {
            using (var streamRead = new StreamReader(path))
            {
                var allLine = streamRead.ReadToEnd();
                var lines = allLine.Split(new[] { ','}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(double.Parse).ToList();
                
                var matrix = new DenseMatrix(lines.Count, 1);
                matrix.SetColumn(0, lines.ToArray());
                var actual = new FileInfo(path).Name.Split('_').First();
                return new StreamData
                {
                    InputDataMatrix = matrix,
                    ExpectedLabel = actual
                };
            }
        }


        private static void AllPreTreatment(string workFolder, string newFolder)
        {
            var dir = new DirectoryInfo(workFolder);
            if (!dir.Exists) return;

            dir.GetFiles()
                .Where(path => path.Extension == ".txt")
                .AsParallel()
                .ForAll((path) =>
                {
                    PreTreatment(path.FullName, newFolder);
                });
        }

        private static void PreTreatment(string path, string newFolder)
        {
            var fileInfo = new FileInfo(path);
            using (var streamWriter = new StreamWriter(Path.Combine(newFolder, fileInfo.Name)))
            using (var streamRead = new StreamReader(path))
            {
                var allData = new List<byte>();
                var allLine = streamRead.ReadToEnd();
                var lines = allLine.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).ToList();
                lines.ForEach(line => { allData.AddRange(line.ToCharArray().Select(a => byte.Parse(a.ToString()))); });

                var mat1 = new Mat(32, 32, MatType.CV_8UC1, allData.ToArray());
                var mat2 = new Mat();
                Cv2.Resize(mat1, mat2, new Size(16, 16));
                mat2.GetArray(out byte[] data2);
                streamWriter.WriteLine(string.Join(",", data2));
            }

        }

    }
}
