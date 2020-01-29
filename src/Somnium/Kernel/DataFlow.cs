using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    public class DataFlow 
    {


        public DataFlow()
        {
            NeureQueue = new Dictionary<int, Neure[]>();
            LayerQueue = new Dictionary<int, Layer>();
        }

        public Matrix InputData { set; get; }

        public Dictionary<int, Layer> LayerQueue { set; get; }
        public Dictionary<int, Neure[]> NeureQueue { set; get; }


        public double[] ExpectedOut { set; get; }
        public double[] ActualOut { set; get; }
        public string ExpectedLabel { set; get; }
        public string ActualLabel { set; get; }

        public void AddLayer(LayerFullConnected layer)
        {
            LayerQueue[layer.LayerIndex] = layer;
            NeureQueue[layer.LayerIndex] = (NeurePerceptron[]) layer.Perceptrons.Clone();
        }

        public void AddLayer(LayerInput layer)
        {
            LayerQueue[layer.LayerIndex] = layer;
            NeureQueue[layer.LayerIndex] = null;
        }
    }


    public class DataUnit<T>
    {
        public DataFormat Format { set; get; }
        public T DataValue { set; get; }

    }

    public enum DataFormat
    {
        MatrixArray,
        Matrix,
        DoubleArray
    }
}
