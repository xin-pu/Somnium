using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Core.Double
{
    public class ActivateNerveLayer : Layer<double>
    {

        public IEnumerable<ActivateNerveCell> ActivateNerveCells { set; get; }

        public ICollection<double> WeightedInput { set; get; }
        public ICollection<double> ActivateOuput { set; get; }


        public ActivateNerveLayer(int layIndex, int nerveCellCount,DataSize datasize)
        {
            LayerColumnIndex = layIndex;
            LayerRowIndex = nerveCellCount;
            ActivateNerveCells = Enumerable.Range(0, nerveCellCount)
                .Select(i => new ActivateNerveCell(datasize));
            DatasOutput=new DenseMatrix(1,nerveCellCount);
        }

        public ActivateNerveLayer(int layIndex, ICollection<ActivateNerveCell> activateNerveCell)
        {
            LayerColumnIndex = layIndex;
            LayerRowIndex = activateNerveCell.Count;
            ActivateNerveCells = activateNerveCell;
            DatasOutput = new DenseMatrix(1, LayerRowIndex);
        }

        public void Activated(Matrix<double> datas)
        {
            WeightedInput = ActivateNerveCells.Select(a => a.Weighted(datas)).ToList();
            ActivateOuput = ActivateNerveCells.Select(a => a.Activated(datas)).ToList();
            DatasOutput.SetRow(0, ActivateOuput.ToArray());
        }


    }
}
