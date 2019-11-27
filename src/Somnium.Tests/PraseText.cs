using System;
using System.IO;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Somnium.Data;

namespace Somnium.Tests
{
    [TestClass]
    public class ParseText
    {
        public string WorkFolder = @"D:\Document Code\Code Somnium\Somnium\datas";

        [TestMethod]
        public void ParseTextToMatrix()
        {
            return;
            var file = Path.Combine(WorkFolder, @"smallDigits\allData.txt");
            using var stream = new StreamReader(file);
            var allLine = stream.ReadToEnd();
            var lines = allLine.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            var allData = lines.ToList().Select(a => a.Split('\t').Select(double.Parse).ToList()).ToList();

            var matrixCount = allData.First().Count() / 6;

            var matrixs = Enumerable.Range(0, matrixCount).ToList().Select((a, b) =>
            {
                var matrix = DenseMatrix.CreateIdentity(6);
                Enumerable.Range(0, 6).ToList().ForEach(row =>
                {
                    matrix.SetRow(row, allData[row].GetRange(a * 6, 6).ToArray());
                });
                return matrix;
            });
            int i = 0;
            int res = 1;
            matrixs.ToList().ForEach(matrix =>
            {
                SaveMatrix.Save(matrix,Path.Combine(WorkFolder, $@"smallDigits\{res}_{i}.txt"));
                i++;
                if (i >= 32)
                {
                    i = 0;
                    res++;
                }
            });
        }
    }
}
