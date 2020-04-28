using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Somnium.Data;
using Somnium.Kernel;

namespace Somnium.Core
{
    [XmlInclude(typeof(LabelMap))]
    [XmlInclude(typeof(LayerInput))]
    [XmlInclude(typeof(LayerOutput))]
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
            InputDataShape = trainDataManager.DataShapeIn;
            OutputDataShape = trainDataManager.DataShapeOut;
            LearningRate = trainParameters.LearningRate;
            CreateLayNet(new LayNetParameter());
        }

        public LayerNetManager(TrainDataManager trainDataManager, TrainParameters trainParameters,
            LayNetParameter layNetParameter)
        {
            LayerNet = new List<Layer>();
            // Loading information from data manager
            LabelMap = trainDataManager.LabelMap;
            InputDataShape = trainDataManager.DataShapeIn;
            OutputDataShape = trainDataManager.DataShapeOut;
            LearningRate = trainParameters.LearningRate;
            CreateLayNet(layNetParameter);
        }

        public LabelMap LabelMap { set; get; }
        public List<Layer> LayerNet { set; get; }
        public double LearningRate { get; }

        private DataShape InputDataShape { get; }
        private int OutputDataShape { get; }


        public void CreateLayNet(LayNetParameter layNetParameter)
        {
            LayerNet = new List<Layer>();
            var inputLayer = new LayerInput(InputDataShape) {LayerIndex = 0};
            LayerNet.Add(inputLayer);
            layNetParameter.FullConnectLayer.ToList().ForEach(a =>
            {
                var dataShape = LayerNet.Last().ShapeOut;
                var fullConnectedLayer = new LayerFullConnected(dataShape, a)
                {
                    LayerIndex = LayerNet.Count
                };
                LayerNet.Add(fullConnectedLayer);
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

    /// <summary>
    /// The LayNet Parameters
    /// </summary>
    public class LayNetParameter
    {
        public int[] FullConnectLayer { set; get; } = new int[0];
    }
}
