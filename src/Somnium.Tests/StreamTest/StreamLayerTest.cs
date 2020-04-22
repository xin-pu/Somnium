using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Data;
using Somnium.Kernel;

namespace Somnium.Tests.StreamTest
{
    [TestClass]
    public class StreamLayerTest
    {

        public string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas\smallDigits";

        
         
        [TestMethod]
        public void TestInitial()
        {
            //定义从文件获取数据流的方法，需要返回矩阵数据以及正确Label
            StreamData.GetStreamData = GetArrayStreamData;

            //读取数据层
            var dir = new DirectoryInfo(WorkFolder);
            var inputStreams = dir.GetFiles()
                .Where(path => path.Extension == ".txt")
                .Select((path, index) =>
                {
                    var inputLayer =StreamData.CreateStreamData(path.FullName);
                    return inputLayer;
                }).ToList();

            var dataShape = StreamData.FilterDataShape(inputStreams);

            //映射标记结果
            var map = new LabelMap(inputStreams.Select(a => a.ActualLabel));
            inputStreams.ForEach(a => a.ActualOut = map.GetCorrectResult(a.ActualLabel));


            //创建神经网络层
            var layerStream = new StreamLayer();
            layerStream.AddInputLayer(new LayerInput(dataShape));
            layerStream.AddFullConnectedLayer(20);
            layerStream.AddOutputLayer(map.Count);

            
            //以神经网络层更新数据层
            inputStreams.ToList().AsParallel().ForAll(singleData =>
            {
                layerStream.RunLayerNet(singleData);
            });

            //从数据层层更新神经网络层的神经元
            layerStream.UpdateWeight(inputStreams);

            //多次迭代

        }

        public StreamData GetStreamData(string path)
        {
            using var streamRead = new StreamReader(path);
            var allLine = streamRead.ReadToEnd();
            var lines = allLine.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var matrix = new DenseMatrix(lines.Count, lines.First().Length);
            var rowIndex = 0;
            lines.ForEach(line =>
            {
                var lineData = line.ToCharArray().Select(a => double.Parse(a.ToString()));
                matrix.SetRow(rowIndex, lineData.ToArray());
                rowIndex++;
            });
            var actual = new FileInfo(path).Name.Split('_').First();
            return new StreamData { InputDataMatrix = matrix, ActualLabel = actual };
        }

        public StreamData GetArrayStreamData(string path)
        {
            using var streamRead = new StreamReader(path);
            var allData = new List<double>();
            var allLine = streamRead.ReadToEnd();
            var lines = allLine.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            lines.ForEach(line => { allData.AddRange(line.ToCharArray().Select(a => double.Parse(a.ToString()))); });

            var matrix = new DenseMatrix(allData.Count, 1);
            matrix.SetColumn(0, allData.ToArray());
            var actual = new FileInfo(path).Name.Split('_').First();
            return new StreamData
            {
                InputDataMatrix = matrix,
                ActualLabel = actual
            };
        }

    }
}
