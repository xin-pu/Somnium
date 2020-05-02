using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using OpenCvSharp;
using Somnium.Core;
using Somnium.Data;
using Somnium.Func;
using Somnium.Kernel;
using Somnium.Learner;

namespace Console
{
    class Program
    {
        private static string workFolder = @"D:\Document Code\Code Somnium\Somnium\datas\resizeDigits";

        static void Main()
        {
            
            for (int i = 0; i < 1000; i++)
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                Thread.Sleep(1);
                stopWatch.Stop();
                System.Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
            }

            //TrainingSmallDigitsWithExtendMethod();
        }


        public static void TrainingSmallDigitsWithExtendMethod()
        {
            // 初始化训练参数
            var trainParameters = new TrainParameters
            {
                LearningRate = 0.005,
                TrainCountLimit = 100,
                CostType = CostType.Basic,
                LikeliHoodType = LikeliHoodType.SoftMax
            };
            trainParameters.AddLayer(LayerType.FullConnectLayer,32);

          

            // 根据数据目录,以及读取文件流的方法，加载数据集
            var trainDataManager = new TrainDataManager(workFolder, new ResizeDigitsDataReader().ReadStreamData);
            trainDataManager.Binding(trainParameters);

            //// 创建训练
            var train = new BasicLearner(trainDataManager, trainParameters);

            // 执行训练
            train.ExecuteTrain();
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
