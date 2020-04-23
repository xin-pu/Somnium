using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    [Serializable]
    public class LayerOutput : Layer
    {
        public int NeureCount {  set; get; }

        public NeurePerceptron[] Perceptrons { set; get; }

        public LayerOutput()
        {
        }

        public LayerOutput(DataShape shape, int neureCount) : base(shape)
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
            var expVal = data.ExpectedOut;
            if (expVal.Length != Perceptrons.Length)
                return;

            var activatedOutputArray = data.GetActivatedArray(LayerIndex);
            var deviations = expVal.Select((a, b) => (activatedOutputArray[b] - a) *
                                                     Perceptrons[b].FirstDerivativeFunc(activatedOutputArray[b]))
                .ToList();
            data.LayerDatas[LayerIndex].Error = deviations;

            data.LayerDatas[LayerIndex - 1].SWd =
                Enumerable.Range(0, ShapeIn.Levels).Select(index =>
                {
                    return Perceptrons.Select((a, b) => a.Weight.At(index, 0) * deviations[b]).Sum();
                });


            var preActivatedMatrix = data.GetActivatedMatrix(LayerIndex - 1);

            Perceptrons.ToList().ForEach(perceptron =>
            {
                var gra = -deviations[perceptron.Order] * gradient;
                perceptron.AddDeviation((Matrix) preActivatedMatrix.Multiply(gra), gra);
            });
        }

        public override void UpdateNeure()
        {
            Perceptrons.ToList().ForEach(a=>a.UpdateDeviation());
        }

        public override void Serializer(string filename)
        {
            using var fs = new FileStream(filename, FileMode.Create);
            new XmlSerializer(typeof(LayerOutput)).Serialize(fs, this);
        }
    }
}
