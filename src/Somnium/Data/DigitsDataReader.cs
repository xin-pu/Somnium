using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Data
{
    public class DigitsDataReader : DataReader
    {
        public override string GetActualLabel(string path)
        {
            var actual = new FileInfo(path).Name.Split('_').First();
            return actual;
        }

        public override Matrix GetMatrixData(string path)
        {
            using var streamRead = new StreamReader(path);
            var allData = new List<double>();
            var allLine = streamRead.ReadToEnd();
            var lines = allLine.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            lines.ForEach(line => { allData.AddRange(line.ToCharArray().Select(a => double.Parse(a.ToString()))); });
            var matrix = new DenseMatrix(allData.Count, 1);
            matrix.SetColumn(0, allData.ToArray());
            return matrix;
        }
    }
}