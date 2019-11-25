using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Core
{

    /// <summary>
    /// Input Layer is the first Layer which the input size and output size are same.
    /// </summary>
    public class InputLayer : StandLayer
    {

        public double[] ExpectVal { set; get; }

        public InputLayer(DataSize inputDataSize) : base(inputDataSize)
        {
            InputDataSizeFormat = OutputDataSizeFormat = inputDataSize;
        }

        public  bool DatasCheckIn(Matrix data, double[] expectedVal)
        {
            var equal = data.RowCount == InputDataSizeFormat.RowCount
                        && data.ColumnCount == InputDataSizeFormat.ColumnCount;
            if (equal)
            {
                InputDatas = OutputDatas = new List<Matrix> {data};
                ExpectVal = expectedVal;
            }

            return equal;
        }

    }
}
