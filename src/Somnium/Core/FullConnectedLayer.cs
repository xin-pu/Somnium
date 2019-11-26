using MathNet.Numerics.LinearAlgebra.Double;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Somnium.Core
{

    /// <summary>
    /// The input data of Full Connected Layer should be Matrix. we need dimensionality reduction.
    /// </summary>
    public class FullConnectedLayer : StandLayer
    {
        public Matrix InputData { set; get; }
        public Matrix OutputData { set; get; }
        public int NerveCellCount { set; get; }
        public IList<ActivateNerveCell> ActivateNerveCells { set; get; }
        public IList<double> WeightedInput { set; get; }
        public IList<double> ActivatedOuput { set; get; }
        public IList<double> Deviations { set; get; }

        public FullConnectedLayer(DataSize inputDataSize, int nerveCellCount) : base(inputDataSize)
        {
            InputDataSizeFormat = inputDataSize;
            OutputDataSizeFormat = new DataSize {DataCount = 1, ColumnCount = 1, RowCount = nerveCellCount};
            OutputData = new DenseMatrix(nerveCellCount, 1);
            NerveCellCount = nerveCellCount;
            ActivateNerveCells = Enumerable.Range(0, nerveCellCount)
                .Select(a => new ActivateNerveCell(new DataSize
                {
                    ColumnCount = 1,
                    RowCount = inputDataSize.Level
                }))
                .ToList();
        }


        public  bool DatasCheckIn(IList<Matrix> datas)
        {
            var equal = datas.Count == InputDataSizeFormat.DataCount &&
                        !datas.Select(a => a.RowCount == InputDataSizeFormat.RowCount).Contains(false) &&
                        !datas.Select(a => a.ColumnCount == InputDataSizeFormat.ColumnCount).Contains(false);
            if (equal)
            {
                InputData = DimensionalityReduction(datas);
                InputDatas = new List<Matrix> { InputData };
                Activated(InputData);
            }

            return equal;
        }

        public bool DatasCheckIn(Matrix data)
        {
            var equal = 1 == InputDataSizeFormat.DataCount
                        && data.RowCount == InputDataSizeFormat.RowCount
                        && data.ColumnCount == InputDataSizeFormat.ColumnCount;
            if (equal)
            {
                InputData = data;
                InputDatas = new List<Matrix> { data };
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

        public void Deviationed(IList<ActivateNerveCell> cells, double gradient)
        {

            var devNextLay = cells.Select((a, b) => a.Deviation * a.Weight.At(b, 0));
            Deviations = ActivateNerveCells.Select((a, b) =>
                devNextLay.Sum() *
                ActivateNerveCells[b].FirstDerivativeFunc(ActivateNerveCells[b].WeightedInput)).ToList();

            var i = 0;
            ActivateNerveCells.ToList().ForEach(a =>
            {
                a.Deviation = Deviations.ElementAt(i);
                var gra = a.Deviation * gradient;
                a.AddDeviation((Matrix)InputData.Multiply(gra), gra);
                i++;
            });
        }

        public void UpdateWeight()
        {
            ActivateNerveCells.ToList().ForEach(a => a.Updated());
        }


        public async Task ActivatedAsync(Matrix datas)
        {
            await Task.Run(() => { Activated(datas); });
        }

        private Matrix DimensionalityReduction(IList<Matrix> datas)
        {
            var datasingles = datas.Select(a => a.Enumerate());
            var datasOutput = new List<double>();
            datasingles.ToList().ForEach(datasingle => datasOutput.AddRange(datasingle));
            return new DenseMatrix(datasOutput.Count, 1, datasOutput.ToArray());
        }


    }
}
