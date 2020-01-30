using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    [Serializable]
    public class LayerFullConnected : Layer
    {

        public int NeureCount { protected set; get; }

        public NeurePerceptron[] Perceptrons { set; get; }

        public LayerFullConnected(DataShape shape, int neureCount) : base(shape)
        {
            ShapeOut = new DataShape(neureCount, 1);
            NeureCount = neureCount;
            Perceptrons = Enumerable.Range(0, neureCount)
                .Select(a => new NeurePerceptron(shape.Levels, 1)
                {
                    Order = a
                })
                .ToArray();
        }



        private Matrix DimensionalityReduction(Matrix[] datas)
        {
            var datasArrays = datas.Select(a => a.Enumerate());
            var datasOutput = new List<double>();
            datasArrays.ToList().ForEach(a => datasOutput.AddRange(a));
            return new DenseMatrix(datasOutput.Count, 1, datasOutput.ToArray());
        }


        public override Array Activated(StreamLayer layerNet, Matrix datas)
        {
            throw new NotImplementedException();
        }

        public override Array Activated(StreamLayer layerNet, Array datas)
        {
            throw new NotImplementedException();
        }
    }
}
