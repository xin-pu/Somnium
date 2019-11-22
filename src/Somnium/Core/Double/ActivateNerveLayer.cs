using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using Somnium.Func;

namespace Somnium.Core.Double
{
    public class ActivateNerveLayer : Layer<double>
    {

        public ICollection<ActivateNerveCell> ActivateNerveCells { set; get; }

        public ICollection<double> WeightedInput { set; get; }
        public ICollection<double> ActivateOuput { set; get; }

            
        public ActivateNerveLayer(DataSize preLayerSize, int outputRows, ActivateFunc func = ActivateFunc.Sigmoid)
            : base(new DataSize {RowCount = outputRows, ColumnCount = 1})
        {
            WeightedInput=new List<double>(outputRows);
            ActivateOuput=new List<double>(outputRows);
            ActivateNerveCells = Enumerable.Range(0, 20).ToList()
                .Select(i => new ActivateNerveCell(preLayerSize)).ToList();
            switch (func)
            {
                case ActivateFunc.Sigmoid:
                    break;
                case ActivateFunc.Tanh:
                    break;
                case ActivateFunc.Max:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(func), func, null);
            }
        }

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
