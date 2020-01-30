using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    [Serializable]
    public class StreamLayer 
    {

        public StreamLayer()
        {
            LayerQueue = new Queue<Layer>();
        }


        public Queue<Layer> LayerQueue { set; get; }
        public Dictionary<int, Neure[]> NeureQueue { set; get; }



        public bool AddInputLayer(LayerInput layer)
        {
            if (LayerQueue.Count != 0) return false;
            layer.LayerIndex = 0;
            LayerQueue.Enqueue(layer);
            return true;
        }

        public bool AddFullConnectedLayer(int neureCount)
        {
            var dataShape = LayerQueue.Last().ShapeOut;
            var fullConnectedLayer = new LayerFullConnected(dataShape, neureCount)
            {
                LayerIndex = LayerQueue.Count
            };
            LayerQueue.Enqueue(fullConnectedLayer);
            return true;
        }

        public bool AddOutputLayer(int neureCount)
        {
            var dataShape = LayerQueue.Last().ShapeOut;
            var outputLayer = new LayerOutput(dataShape, neureCount)
            {
                LayerIndex = LayerQueue.Count
            };
            LayerQueue.Enqueue(outputLayer);
            return true;
        }

        public void ClearLayer()
        {
            LayerQueue.Clear();
        }

        public void ResetLayer()
        {
        }

        public bool CheckLayer(Matrix inputData)
        {
            return false;
        }

        public double[] RunLayerNet(Matrix inputdata)
        {
            return new double[0];
        }


    }


  
}
