using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Core
{
    public class ActivateNerveLayer : StandLayer
    {

        public ICollection<ActivateNerveCell> ActivateNerveCells { set; get; }
        public ICollection<double> WeightedInput { set; get; }
        public ICollection<double> ActivateOuput { set; get; }
        
        public int NerveCellCount { set; get; }

        public ActivateNerveLayer(int nerveCellCount, DataSize fitSize)
            : base(new DataSize { RowCount = nerveCellCount, ColumnCount = 1 })
        {
            WeightedInput = new List<double>(nerveCellCount);
            ActivateOuput = new List<double>(nerveCellCount);
           
        }

        public void CheckInNerveLayer(ActivateNerveCell activateNerveCell)
        {
            ActivateNerveCells = Enumerable.Range(0, NerveCellCount).ToList()
                .Select(i => new ActivateNerveCell(DataSizeInputFormat)).ToList();
        }

        public void Activated(Matrix datas)
        {
            WeightedInput = ActivateNerveCells.Select(a => a.Weighted(datas)).ToList();
            ActivateOuput = ActivateNerveCells.Select(a => a.Activated(datas)).ToList();
            DatasOutput.SetRow(0, ActivateOuput.ToArray());
        }

        public async Task ActivatedAsync(Matrix datas)
        {
            await Task.Run(() => { Activated(datas); });
        }

    
    }
}
