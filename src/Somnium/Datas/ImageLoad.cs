using System;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Core.Double;

namespace Somnium.Datas
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

                var lines = allline.Split(new []{'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).ToList();
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
                return new InputLayer(matrix, double.Parse(excepted)) {Name = file.Name};
            }
        }
    }
}
