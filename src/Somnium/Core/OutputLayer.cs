using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Func;

namespace Somnium.Core
{
    public class OutputLayer : StandLayer
    {

        public Matrix InputData { set; get; }
        public Matrix OutputData { set; get; }
        public int NerveCellCount { set; get; }


        public IList<ActivateNerveCell> ActivateNerveCells { set; get; }
        public IList<double> WeightedInput { set; get; }
        public IList<double> ActivatedOuput { set; get; }
        public IList<double> Probability { set; get; }
        public IList<double> Deviations { set; get; }

        public double Variance { set; get; }

        public OutputLayer(DataSize inputDataSize, int nerveCellCount) : base(inputDataSize)
        {
            NerveCellCount = nerveCellCount;
            InputDataSizeFormat = inputDataSize;
            OutputDataSizeFormat = new DataSize {ColumnCount = 1, RowCount = nerveCellCount, DataCount = 1};
            OutputData = new DenseMatrix(nerveCellCount, 1);
            ActivateNerveCells = Enumerable.Range(0, nerveCellCount)
                .Select(a => new ActivateNerveCell(InputDataSizeFormat)).ToList();
        }


        public bool DatasCheckIn(Matrix data)
        {
            var equal = 1 == InputDataSizeFormat.DataCount
                        && data.RowCount == InputDataSizeFormat.RowCount
                        && data.ColumnCount == InputDataSizeFormat.ColumnCount;
            if (equal)
            {
                InputData = data;
                InputDatas = new List<Matrix> {data};
                Activated(InputData);
            }
            return equal;
        }


        public void Activated(Matrix datas)
        {
            ActivateNerveCells.ToList().ForEach(a => { a.Activated(datas); });

            WeightedInput = ActivateNerveCells.Select(a => a.WeightedInput).ToList();
            ActivatedOuput = ActivateNerveCells.Select(a => a.ActivateOuput).ToList();

            OutputData.SetColumn(0, ActivatedOuput.ToArray());
            OutputDatas = new List<Matrix> {OutputData};

        }

        public void Deviationed(IList<double> expectedVal,double gradient)
        {
            if (expectedVal.Count != OutputData.RowCount)
                return;
            Deviations = expectedVal.Select((a, b) =>
                (OutputData[b, 0] - a) *
                ActivateNerveCells[b].FirstDerivativeFunc(ActivateNerveCells[b].WeightedInput)).ToList();

            var i = 0;
            ActivateNerveCells.ToList().ForEach(a =>
            {
                a.Deviation = Deviations.ElementAt(i);
                var gra = -a.Deviation * gradient;
                a.AddDeviation((Matrix) InputData.Multiply(gra), gra);
                i++;
            });
        }

        public void UpdateWeight()
        {
           ActivateNerveCells.ToList().ForEach(a=>a.Updated());
        }

        public double GetVariance(IList<double> expectedVal)
        {
            if (expectedVal.Count != OutputData.RowCount)
                return double.NaN;
            var variance = expectedVal.Zip(OutputData.Column(0).AsArray(), (a, b) => Math.Pow((a - b), 2)).Sum() / 2;
            return variance;
        }

        public IList<double> GetLikelihoodRatio()
        {
            return SoftMax.softMax(OutputData.Enumerate().ToList());
        }
    }
}
