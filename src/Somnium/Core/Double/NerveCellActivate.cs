using System;
using System.Linq;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Core.Double
{
    public class NerveCellActivate : NerveCell<double>
    {

        private Func<double, double> activateFuc;

        public NerveCellActivate(int rows, int columns)
        {
            Weight = DenseMatrix.CreateRandom(rows, columns, new ContinuousUniform());
            Bias = new ContinuousUniform().Median;
        }

        public override Func<double, double> ActivateFuc
        {
            get { return activateFuc; }
            set
            {
                activateFuc = value;
                DeltaActivateFuc = MathNet.Numerics.Differentiate.FirstDerivativeFunc(ActivateFuc);
            }
        }

        public override Func<double, double> DeltaActivateFuc { set; get; }

        public override double ActivateNerveCell(Layer<double> inputLayer)
        {
            if (inputLayer.ColumnCount != Weight.ColumnCount || inputLayer.RowCount != Weight.RowCount)
                return double.NaN;
            return ActivateFuc(inputLayer.DatasInput.PointwiseMultiply(Weight).Enumerate().Sum() + Bias);
        }

    }
}
