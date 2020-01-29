using System;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    /// <summary>
    /// Input Layer is the first Layer which the input size and output size are same.
    /// </summary>
    [Serializable]
    public class LayerInput : Layer
    {


        public LayerInput(int rows, int columns, int layers, int layerIndex = 0)
            : base(rows, columns, layers, layerIndex)
        {

        }

        public LayerInput(DataShape shape, int layerIndex = 0)
            : base(shape,layerIndex)
        {
          
        }


        public override Array Activated(DataFlow dataFlow, Matrix datas)
        {
            throw new NotImplementedException();
        }

        public override Array Activated(DataFlow dataFlow, Array datas)
        {
            throw new NotImplementedException();
        }
    }
}
