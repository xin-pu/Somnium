using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Core;

namespace Somnium.Kernel
{
    public class LayerFullConnected : Layer
    {

        public int NeureCount { set; get; }


        public Perceptron[] Perceptrons { set; get; }

        public LayerFullConnected(DataShape shape, int neureCount,int layerIndex)
        {
            LayerIndex = layerIndex;
            ShapeIn = shape;
            ShapeOut = new DataShape {Rows = neureCount, Columns = 1, Layers = 1};
            Perceptrons = Enumerable.Range(0, neureCount)
                .Select(a => new Perceptron(shape.Levels, 1)
                {
                    LayerIndex = layerIndex,
                    Order = a
                })
                .ToArray();
        }

        public override void Save(string path)
        {
            using var fs = new FileStream(path, FileMode.Create);
            var formatter = new XmlSerializer(typeof(FullConnectedLayer));
            formatter.Serialize(fs, this);
        }

        public override bool CheckInData(DataFlow dataFlow)
        {
            var dataIn = dataFlow.Data;
            var columns = dataIn.Select(a => a.ColumnCount).Distinct().ToArray();
            var rows= dataIn.Select(a => a.RowCount).Distinct().ToArray();
            if (columns.Length != 1 || rows.Length != 1) return false;
            var equal = dataIn.Length == ShapeIn.Layers &&
                        rows.First() == ShapeIn.Rows &&
                        columns.First() == ShapeIn.Columns;
            if (equal)
            {
               var DataIn = DimensionalityReduction(dataIn);
            }

            return equal;
        }


        private Matrix DimensionalityReduction(Matrix[] datas)
        {
            var datasArrays = datas.Select(a => a.Enumerate());
            var datasOutput = new List<double>();
            datasArrays.ToList().ForEach(a => datasOutput.AddRange(a));
            return new DenseMatrix(datasOutput.Count, 1, datasOutput.ToArray());
        }



    }
}
