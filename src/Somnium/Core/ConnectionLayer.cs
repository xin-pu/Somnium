using MathNet.Numerics.LinearAlgebra.Double;
using System.Collections.Generic;

namespace Somnium.Core
{
    public class ConnectionLayer : StandLayer
    {

        public ICollection<ActivateNerveCell> ActivateNerveCells { set; get; }

        public ConnectionLayer(DataSize dataSize) : base(dataSize)
        {

        }




    }
}
