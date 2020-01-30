using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    public class LayerOutput : Layer
    {
        public int NeureCount { protected set; get; }

        public NeurePerceptron[] Perceptrons { set; get; }

        public LayerOutput(int rows, int columns, int layers) : base(rows, columns, layers)
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
