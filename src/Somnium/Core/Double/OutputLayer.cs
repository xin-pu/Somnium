using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Core.Double
{
    public class OutputLayer : Layer<double>
    {

        public IEnumerable<ActivateNerveCell> ActivateNerveCells { set; get; }

        public ICollection<double> WeightedInput { set; get; }
        public ICollection<double> ActivateOuput { set; get; }

        public ICollection<double> RightOuput { set; get; }

        public OutputLayer(DataSize dataSize) : base(dataSize)
        {
        }


        //        public OutputLayer(int layIndex, int nerveCellCount, DataSize datasize)
        //        {
        //            LayerColumnIndex = layIndex;
        //            LayerRowIndex = nerveCellCount;
        //            ActivateNerveCells = Enumerable.Range(0, nerveCellCount)
        //                .Select(i => new ActivateNerveCell(datasize));
        //            DatasOutput = new DenseMatrix(1, nerveCellCount);
        //        }
        //
        //        public OutputLayer(int layIndex, ICollection<ActivateNerveCell> nerveCellCounts)
        //        {
        //            LayerColumnIndex = layIndex;
        //            LayerRowIndex = nerveCellCounts.Count;
        //            ActivateNerveCells = nerveCellCounts;
        //            DatasOutput = new DenseMatrix(1, LayerRowIndex);
        //        }

        public void Activated(Matrix<double> datas)
        {
            WeightedInput = ActivateNerveCells.Select(a => a.Weighted(datas)).ToList();
            ActivateOuput = ActivateNerveCells.Select(a => a.Activated(datas)).ToList();
            DatasOutput.SetRow(0, ActivateOuput.ToArray());
        }

        public async Task ActivatedAsync(Matrix<double> datas)
        {
            await Task.Run(() => { Activated(datas); });
        }

   
    }
}
