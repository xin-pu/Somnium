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
            var ActivateOuput = ActivateNerveCells.Select(a => a.ActivateOuput);
            OutputData.SetColumn(0, ActivateOuput.ToArray());
            OutputDatas = new List<Matrix> {OutputData};
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
