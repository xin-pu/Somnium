using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Core.Double
{
    public class LayerActivate : Layer<double>
    {


        public NerveCellActivate NerveCell { set; get; }

        public LayerActivate(Matrix<double> datas) : base(datas)
        {
            
        }
    }
}
