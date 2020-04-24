using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{

    public class LayerOutput : Layer
    {
        private NeurePerceptron[] _perceptrons;
        public int NeureCount => Perceptrons.Length;

        public NeurePerceptron[] Perceptrons
        {
            set => UpdateProperty(ref _perceptrons, value);
            get => _perceptrons;
        }

        public LayerOutput()
        {
        }

        /// <summary>
        /// Output layer's output shape should be (NeureCount * 1)
        /// And the Neure Count is the count of results.
        /// [ a(1) ]
        /// [ a(2) ]
        /// ...
        /// [ a(n-1) 
        /// [ a(n) ]
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="neureCount"></param>
        public LayerOutput(DataShape shape, int neureCount) : base(shape)
        {
            ShapeIn = shape;
            ShapeOut = new DataShape(neureCount, 1);
            Perceptrons = Enumerable.Range(0, neureCount)
                .Select(a => new NeurePerceptron(shape.Levels, 1)
                {
                    Order = a
                })
                .ToArray();
        }

        /// <summary>
        /// Activated will update input data by layer's Perceptrons
        /// </summary>
        /// <param name="datas"></param>
        /// <returns>Tuple which Item1 is Activated, and Item2 is Weighted</returns>
        public override Tuple<Matrix, Matrix> Activated(Matrix datas)
        {
            var activatedRes = Perceptrons.Select(perceptron => perceptron.Activated(datas)).ToArray();
            var activatedWithActivated = activatedRes.Select(a => a.Item1).ToArray();
            var activatedWithWeighted = activatedRes.Select(a => a.Item2).ToArray();
            
            return new Tuple<Matrix, Matrix>(
                new DenseMatrix(ShapeOut.Rows, ShapeOut.Columns, activatedWithActivated),
                new DenseMatrix(ShapeOut.Rows, ShapeOut.Columns, activatedWithWeighted));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="gradient"></param>
        public override void Deviated(StreamData data, double gradient)
        {
            var expVal = data.ExpectedOut;
            if (expVal.Length != Perceptrons.Length)
                return;

            var activatedOutputArray = data.GetActivatedArray(LayerIndex);
            var preActivatedMatrix = data.GetActivatedMatrix(LayerIndex - 1);

            var deviations = expVal
                .Select((a, b) =>
                    (activatedOutputArray[b] - a) * Perceptrons[b].FirstDerivativeFunc(activatedOutputArray[b]))
                .ToList();

            data.LayerDatas[LayerIndex].Error = deviations;


            data.LayerDatas[LayerIndex - 1].SWd =
                Enumerable.Range(0, ShapeIn.Levels)
                    .Select(index =>
                    {
                        return Perceptrons.Select((a, b) => a.Weight.At(index, 0) * deviations[b])
                            .Sum();
                    });


            Perceptrons.ToList().ForEach(perceptron =>
            {
                var gra = -deviations[perceptron.Order] * gradient;
                perceptron.AddDeviation((Matrix) preActivatedMatrix.Multiply(gra), gra);
            });
        }

        public override void UpdateNeure()
        {
            Perceptrons.AsParallel().ToList()
                .ForEach(a => a.UpdateDeviation());
        }

        public override void Serializer(string filename)
        {
            using var fs = new FileStream(filename, FileMode.Create);
            new XmlSerializer(typeof(LayerOutput)).Serialize(fs, this);
        }

        public static Layer Deserialize(string filename)
        {
            using var fs = new FileStream(filename, FileMode.Open);
            return (LayerOutput)(new XmlSerializer(typeof(LayerOutput)).Deserialize(fs));
        }
    }
}
