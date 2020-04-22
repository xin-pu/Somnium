using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    /// <summary>
    /// FullConnected Layer is the main Layer for deep learning.
    /// It need define data shape and the neure count.
    /// So Activated method will return the input data.
    /// </summary>
    [Serializable]
    public class LayerFullConnected : Layer
    {

        public int NeureCount { protected set; get; }

        public NeurePerceptron[] Perceptrons { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="neureCount"></param>
        public LayerFullConnected(DataShape shape, int neureCount) : base(shape)
        {
            ShapeIn = shape;
            // Full connected layer's output shape should be (NeureCount * 1)
            // [ a(1) ]
            // [ a(2) ]
            // ...
            // [ a(n-1) ]
            // [ a(n) ]
            NeureCount = neureCount;

            Perceptrons = Enumerable.Range(0, neureCount)
                .Select(a => new NeurePerceptron(shape.Levels, 1)
                {
                    Order = a
                })
                .ToArray();

            ShapeOut = new DataShape(neureCount, 1);

        }

        public override Tuple<Matrix, Matrix> Activated(Matrix datas)
        {
            var activatedRes = Perceptrons.Select(perceptron => perceptron.Activated(datas)).ToArray();
            var activatedWithActivated = activatedRes.Select(a => a.Item1).ToArray();
            var activatedWithWeighted = activatedRes.Select(a => a.Item2).ToArray();

            return new Tuple<Matrix, Matrix>(
                new DenseMatrix(ShapeOut.Rows, ShapeOut.Columns, activatedWithActivated),
                new DenseMatrix(ShapeOut.Rows, ShapeOut.Columns, activatedWithWeighted));
        }


        public override void Deviated(StreamData data, double gradient)
        {
            var expVal = data.QueueActivated[LayerIndex].AsRowMajorArray();
            if (expVal.Length != Perceptrons.Length)
                return;
            var weightOutput = data.QueueWeighted[LayerIndex];
            var activatedOutput = data.QueueActivated[LayerIndex];
            var deviations = expVal.Select((a, b) =>
                (activatedOutput[b, 0] - a) * Perceptrons[b].FirstDerivativeFunc(weightOutput[b, 0])).ToList();

            var i = 0;
            Perceptrons.ToList().ForEach(perceptron =>
            {
                var deviation = deviations.ElementAt(i);
                var gra = -deviation * gradient;
                perceptron.AddDeviation((Matrix)data.QueueActivated[LayerIndex - 1].Multiply(gra), gra);
                i++;
            });
        }

        public override void UpdateNeure()
        {
            Perceptrons.ToList().ForEach(a => a.UpdateDeviation());
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
