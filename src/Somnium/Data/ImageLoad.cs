using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Core;

namespace Somnium.Data
{
    public class ImageLoad
    {
        public static InputLayer ReadLayerInput(string path, Func<string, InputLayer> func)
        {
            return func(path);
        }

        public static InputLayer ReadLayerInput(string path)
        {
            using (var streamRead = new StreamReader(path))
            {
                var allline = streamRead.ReadToEnd();

                var lines = allline.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var matrix = new DenseMatrix(lines.Count, lines.First().Length);
                var rowIndex = 0;
                lines.ForEach(line =>
                {
                    var linedata = line.ToCharArray().Select(a => double.Parse(a.ToString()));
                    matrix.SetRow(rowIndex, linedata.ToArray());
                    rowIndex++;
                });
                var file = new FileInfo(path);
                var excepted = file.Name.Split('_').First();
                var exceptedDatas = new double[3];
                exceptedDatas[int.Parse(excepted) - 1] = 1;
                var inputLayer= new InputLayer(
                    new DataSize { RowCount = matrix.RowCount, ColumnCount = matrix.ColumnCount });
                inputLayer.DatasCheckIn(matrix, exceptedDatas);
                return inputLayer;
            }
        }

        public static IList<InputLayer> ReadLayerInputs(string path)
        {
            var files = new DirectoryInfo(path).GetFiles();
            return files.Select(a => ReadLayerInput(a.FullName)).ToList();
        }
    }
}
