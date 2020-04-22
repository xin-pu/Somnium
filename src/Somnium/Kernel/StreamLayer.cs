﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public Dictionary<int, Neure[]> NeureQueue { set; get; }
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
            LayerQueue.Clear();
        }

        public void ResetLayer()
        {
        }

        public bool CheckLayer(Matrix inputData)
        {
            return false;
        }

        public double[] RunLayerNet(StreamData streamData)
        {
            var tempData = streamData.InputDataMatrix;
            LayerQueue.ToList().ForEach(layer =>
            {
                var (item1, item2) = layer.Activated(tempData);
                streamData.QueueWeighted.Add(item2);
                streamData.QueueActivated.Add(item1);
                tempData = item1;
            });
            return new double[0];
        }

        public void UpdateWeight(List<StreamData> streamDatas)
        {
            LayerQueue.OrderByDescending(a => a.LayerIndex).ToList().ForEach(layer =>
            {
                streamDatas.ForEach(streamData => { layer.Deviated(streamData, Gradient); });
                layer.UpdateNeure();
            });
        }



    }



}
