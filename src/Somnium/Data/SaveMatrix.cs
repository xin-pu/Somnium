using System.IO;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Data
{
    public class SaveMatrix
    {
        public static void Save(Matrix data, string path, string separator = "")
        {
            using var sw = new StreamWriter(path);
            var rows = data.ToRowArrays();
            foreach (var row in rows)
            {
                sw.WriteLine(string.Join(separator, row));
            }
        }
    }
}
