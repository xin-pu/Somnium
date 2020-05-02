using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Somnium.Kernel;

namespace Somnium.Core
{
    [XmlInclude(typeof(LabelMap))]
    [XmlInclude(typeof(LayerInput))]
    [XmlInclude(typeof(LayerOutput))]
    [XmlInclude(typeof(LayerPooling))]
    [XmlInclude(typeof(LayerFullConnected))]
    public class LayerNetManager
    {

        public LayerNetManager()
        {
            LayerNet = new List<Layer>();
        }


        public LayerNetManager(TrainDataManager trainDataManager, TrainParameters trainParameters)
        {
            LayerNet = new List<Layer>();
            TrainParameters = trainParameters;
            LabelMap = trainDataManager.LabelMap;
            InputDataShape = trainDataManager.DataShapeIn;
            OutputDataShape = trainDataManager.DataShapeOut;
            LearningRate = trainParameters.LearningRate;
            CreateLayNet(trainParameters);
        }

        public LabelMap LabelMap { set; get; }
        public List<Layer> LayerNet { set; get; }
        public double LearningRate { get; }
        private DataShape InputDataShape { get; }
        private int OutputDataShape { get; }
        private TrainParameters TrainParameters { set; get; }

        public void CreateLayNet(TrainParameters layNetParameter)
        {
            LayerNet = new List<Layer>();
            var inputLayer = new LayerInput(InputDataShape) {LayerIndex = 0};
            LayerNet.Add(inputLayer);
            layNetParameter.InterLayerStructs.ToList().ForEach(a =>
            {
                var dataShape = LayerNet.Last().ShapeOut;
                switch (a.LayerType)
                {
                    case LayerType.FullConnectLayer:
                        var fullConnectedLayer = new LayerFullConnected(dataShape, a.NeureCount)
                        {
                            LayerIndex = LayerNet.Count
                        };
                        LayerNet.Add(fullConnectedLayer);
                        break;
                    case LayerType.PoolingLayer:
                        var poolingLayer = new LayerPooling(dataShape)
                        {
                            LayerIndex = LayerNet.Count
                        };
                        LayerNet.Add(poolingLayer);
                        break;
                }
            });
            var outputLayer = new LayerOutput(LayerNet.Last().ShapeOut, OutputDataShape)
            {
                LayerIndex = LayerNet.Count
            };
            LayerNet.Add(outputLayer);
        }


        public void UpdateWeight()
        {
            LayerNet.ToList().ForEach(layer => { layer.UpdateNeure(); });
        }

        public void Serializer(string path)
        {
            using var fs = new FileStream(path, FileMode.Create);
             new XmlSerializer(typeof(LayerNetManager)).Serialize(fs, this);
        }

        public static LayerNetManager Deserialize(string path)
        {
            using var fs = new FileStream(path, FileMode.Open);
            return (LayerNetManager) (new XmlSerializer(typeof(LayerNetManager)).Deserialize(fs));
        }

    }

 
}
