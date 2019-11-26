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
            DeltaWeight = DenseMatrix.CreateDiagonal(datasize.RowCount, datasize.ColumnCount, 0);
            DeltaBias = 0;
            ActivateFuc = Activate.Sigmoid;
        }

        public double WeightedInput { set; get; }
        public double ActivateOuput { set; get; }
        public double Deviation { set; get; }

        public Func<double, double> ActivateFuc
        {
            get { return activateFuc; }
            set
            {
                activateFuc = value;
                FirstDerivativeFunc = MathNet.Numerics.Differentiate.FirstDerivativeFunc(ActivateFuc);
            }
        }

        public Func<double, double> FirstDerivativeFunc { set; get; }

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

        public void Updated()
        {
            Weight = (Matrix) (Weight + DeltaWeight);
            Bias += DeltaBias;
            DeltaWeight.Clear();
            DeltaBias = 0;
        }

        public void AddDeviation(Matrix devWeight, double devBias)
        {
            DeltaWeight = (Matrix) (DeltaWeight + devWeight);
            DeltaBias = DeltaBias + devBias;
        }
    }
}
