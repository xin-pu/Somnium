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

        public double Variance { set; get; }
        public IList<double> RightOuput { set; get; }
        public IList<double> Probability { set; get; }


        public OutputLayer(DataSize inputDataSize, int nerveCellCount) : base(inputDataSize)
        {
            NerveCellCount = nerveCellCount;
            InputDataSizeFormat = inputDataSize;
            OutputDataSizeFormat = new DataSize {ColumnCount = 1, RowCount = nerveCellCount, DataCount = 1};
            OutputData = new DenseMatrix(nerveCellCount, 1);
            ActivateNerveCells = Enumerable.Range(0, nerveCellCount)
                .Select(a => new ActivateNerveCell(InputDataSizeFormat)).ToList();
        }


        public bool DatasCheckIn(Matrix data,IList<double> expectedRes)
        {
            var equal = 1 == InputDataSizeFormat.DataCount
                        && data.RowCount == InputDataSizeFormat.RowCount
                        && data.ColumnCount == InputDataSizeFormat.ColumnCount;
            if (equal)
            {
                InputData = data;
                InputDatas = new List<Matrix> { data };
                Activated(InputData);
                Variance = GetVariance(expectedRes);
            }

            return equal;
        }

        public void Activated(Matrix datas)
        {
            ActivateNerveCells.ToList().ForEach(a => { a.Activated(datas); });
            var ActivateOuput = ActivateNerveCells.Select(a => a.ActivateOuput);
            OutputData.SetColumn(0, ActivateOuput.ToArray());
            OutputDatas = new List<Matrix> {OutputData};
            Probability = GetLikelihoodRatio();
        }

        public async Task ActivatedAsync(Matrix datas)
        {
            await Task.Run(() => { Activated(datas); });
        }


        private double GetVariance(IList<double> expectedVal)
        {
            if (expectedVal.Count != OutputData.RowCount)
                return double.NaN;
            var variance = expectedVal.Zip(OutputData.Column(0).AsArray(), (a, b) => Math.Pow((a - b), 2)).Sum() / 2;
            return variance;
        }

        private IList<double> GetLikelihoodRatio()
        {
            return SoftMax.softMax(OutputData.Enumerate().ToList());
        }
    }
}
