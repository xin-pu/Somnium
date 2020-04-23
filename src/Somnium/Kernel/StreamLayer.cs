using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    [Serializable]
    public class StreamLayer
    {

        public StreamLayer(double gradient = 0.01)
        {
            LayerQueue = new Queue<Layer>();
            Gradient = gradient;
        }


        public Queue<Layer> LayerQueue { set; get; }
        public double Gradient { get; }
        public double Target { get; }

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
            
        }

        public void SaveLayer(string path)
        {
            //using var fs = new FileStream(path, FileMode.Create);
            //var serializer = new BinaryFormatter();
            //serializer.Serialize(fs, this);
        }


        public void UpdateWeight(List<StreamData> streamDatas)
        {
            LayerQueue.ToList().ForEach(layer =>
            {
                layer.UpdateNeure();
            });
        }



    }



}
