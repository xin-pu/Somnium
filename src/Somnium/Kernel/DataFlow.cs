using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    public class DataFlow
    {
        public DataFlow()
        {
            DataQueuePre = new Dictionary<int, Matrix[]>();
            DataQueuePost = new Dictionary<int, double[]>();
        }

        public Dictionary<int, Matrix[]> DataQueuePre { set; get; }
        public Dictionary<int, double[]> DataQueuePost { set; get; }

        public double[] ExpectedOut { set; get; }
        public double[] ActualOut { set; get; }
        public string ExpectedLabel { set; get; }
        public string ActualLabel { set; get; }



    }
}
