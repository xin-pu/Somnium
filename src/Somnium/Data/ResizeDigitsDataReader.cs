using System;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Data
{
    public class ResizeDigitsDataReader : DataReader
    {
        public override string GetActualLabel(string path)
        {
            var actual = new FileInfo(path).Name.Split('_').First();
            return actual;
        }

        public override Matrix GetMatrixData(string path)
        {
            using var streamRead = new StreamReader(path);
            var allLine = streamRead.ReadToEnd();
            var lines = allLine.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Select(double.Parse).ToList();

            var matrix = new DenseMatrix(lines.Count, 1);
            matrix.SetColumn(0, lines.ToArray());
            return matrix;
        }
    }
}