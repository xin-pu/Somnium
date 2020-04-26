using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Core;

namespace Somnium.Tests.StreamTest
{
    [TestClass]
    public class StreamLayerTest
    {

        public string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas\smallDigits";




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
            return new StreamData { InputDataMatrix = matrix, ExpectedLabel = actual };
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
                ExpectedLabel = actual
            };
        }

    }
}
