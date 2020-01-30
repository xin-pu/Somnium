using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    /// <summary>
    /// Input Layer is the first Layer which the input size and output size are same.
    /// </summary>
    [Serializable]
    public class LayerInput : Layer
    {


        public LayerInput(int rows, int columns, int layers=1)
            : base(rows, columns, layers)
        {

        }

        public LayerInput(DataShape shape)
            : base(shape)
        {
          
        }


        public override Array Activated(StreamLayer layerNet, Matrix datas)
        {
            var values = datas.AsRowMajorArray();
            return values.ToArray();
        }

        public override Array Activated(StreamLayer layerNet, Array datas)
        {
            return datas;
        }
    }
}
