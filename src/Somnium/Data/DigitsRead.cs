using System;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Core;

namespace Somnium.Data
{
    public class DigitsRead : IStreamLoad
    {

        public DigitsRead()
        {

        }

        public StreamData ReadStreamData(string path)
        {
            using var streamRead = new StreamReader(path);
            var allLine = streamRead.ReadToEnd();
            var lines = allLine.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Select(double.Parse).ToList();

            var matrix = new DenseMatrix(lines.Count, 1);
            matrix.SetColumn(0, lines.ToArray());
            var actual = new FileInfo(path).Name.Split('_').First();
            return new StreamData(matrix, actual);
        }
    }
}