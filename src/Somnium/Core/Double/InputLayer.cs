using System.Collections.Generic;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;

namespace Somnium.Core.Double
{
    public class InputLayer : Layer<double>
    {


        public double ExpectVal { set; get; }

        public InputLayer(Matrix<double> datas, double expectVal)
        {
            LayerColumnIndex = 1;
            
            DatasOutput = datas;
            ExpectVal = expectVal;
        }

        public InputLayer(DataSize dataSize)
        {
            DataSize = dataSize;
        }
    }
}
