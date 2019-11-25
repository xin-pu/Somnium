using System;
using System.Linq;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Func;

namespace Somnium.Core
{
    public class ActivateNerveCell : NerveCell
    {

        private Func<double, double> activateFuc;

        public ActivateNerveCell(DataSize datasize) : base(datasize)
        {
            Weight = DenseMatrix.CreateRandom(datasize.RowCount, datasize.ColumnCount, new ContinuousUniform());
            Bias = new ContinuousUniform().Median;
            ActivateFuc = Activate.Sigmoid;
        }

        public double WeightedInput { set; get; }
        public double ActivateOuput { set; get; }

        public Func<double, double> ActivateFuc
        {
            get { return activateFuc; }
            set
            {
                activateFuc = value;
                DeltaActivateFuc = MathNet.Numerics.Differentiate.FirstDerivativeFunc(ActivateFuc);
            }
        }

        public Func<double, double> DeltaActivateFuc { set; get; }

        public void Activated(Matrix inputData)
        {
            WeightedInput = inputData.PointwiseMultiply(Weight).Enumerate().Sum();
            ActivateOuput = ActivateFuc(inputData.PointwiseMultiply(Weight).Enumerate().Sum() + Bias);
        }

        public double GetActivated(Matrix inputData)
        {
            var weightedInput = inputData.PointwiseMultiply(Weight).Enumerate().Sum();
            var activateOuput = ActivateFuc(weightedInput + Bias);
            return activateOuput;
        }
    }
}
