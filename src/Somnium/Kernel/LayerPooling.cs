using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Core;

namespace Somnium.Kernel
{
    public class LayerPooling : Layer
    {
        private int hen;
        private int dis;

        private PoolingParameter _poolingParameter = new PoolingParameter
        {
            PoolingFunc = PoolingFunc.MaxPool,
            K = 2,
            S = 2
        };

        public PoolingParameter PoolingParameter
        {
            set => UpdateProperty(ref _poolingParameter, value);
            get => _poolingParameter;
        }


        public LayerPooling()
        {
        }

        public LayerPooling(DataShape shape)
        {
            hen = PoolingParameter.K;
            dis = PoolingParameter.S;

            ShapeIn = shape;
            var columns = (ShapeIn.Columns - dis) / hen + 1;
            var rows = (ShapeIn.Rows - dis) / hen + 1;

            ShapeOut = new DataShape(rows, columns);
        }

        public override Tuple<Matrix, Matrix> Activated(Matrix datas)
        {

            var newMatrix = DenseMatrix.Create(ShapeOut.Rows, ShapeOut.Columns, 0);
            for (var i = 0; i < ShapeOut.Rows; i++)
            {
                var columnData = Enumerable.Range(0, ShapeOut.Columns)
                    .ToList()
                    .Select(col => datas
                        .SubMatrix(i * dis, hen, col * dis, hen));
                var array = columnData.Select(a => a.Enumerate().Max()).ToArray();
                newMatrix.SetRow(i, array);
            }

            return new Tuple<Matrix, Matrix>(newMatrix, newMatrix);
        }

        public override void Deviated(StreamData data, double gradient)
        {
            // Do nothing here
        }

        public override void UpdateNeure()
        {
            // Do nothing here
        }

        public override void Serializer(string filename)
        {
            using var fs = new FileStream(filename, FileMode.Create);
            new XmlSerializer(typeof(LayerPooling)).Serialize(fs, this);
        }
    }

    public class PoolingParameter
    {
        public PoolingFunc PoolingFunc { set; get; }
        public int K { set; get; }
        public int S { set; get; }
    }

    public enum PoolingFunc
    {
        MaxPool,
    }
}
