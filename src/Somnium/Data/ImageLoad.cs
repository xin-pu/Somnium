using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Core;
using Somnium.Kernel;

namespace Somnium.Data
{
    public class ImageLoad
    {
        public static InputLayer ReadLayerInput(string path, Func<string, InputLayer> func)
        {
            return func(path);
        }

        public static IList<InputLayer> ReadLayerInputs(string folder, Func<string, InputLayer> func)
        {
            var files = new DirectoryInfo(folder).GetFiles();
            return files.Select(a => ReadLayerInput(a.FullName, func)).ToList();
        }

        public static InputLayer ReadDigitsAsInputLayer(string path)
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
            var excepted = new FileInfo(path).Name.Split('_').First();
            var inputLayer = new InputLayer(
                new DataSize { RowCount = matrix.RowCount, ColumnCount = matrix.ColumnCount });
            inputLayer.DatasCheckIn(matrix, excepted);
            return inputLayer;
        }


    }
}
