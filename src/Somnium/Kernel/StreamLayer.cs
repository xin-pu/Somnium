using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Somnium.Kernel
{
    [XmlInclude(typeof(LayerInput))]
    [XmlInclude(typeof(LayerOutput))]
    [XmlInclude(typeof(LayerFullConnected))]
    public class StreamLayer
    {

        public StreamLayer()
        {
            LayerQueue = new List<Layer>();
        }




        public StreamLayer(DataShape inputDataShape, int outputSize, int[] neureList, double gradient = 0.01)
        {
            LayerQueue = new List<Layer>();
            InputDataShape = inputDataShape;
            OutputDataShape = outputSize;
            Gradient = gradient;
            CreateFullConnectLayer(neureList);
        }

        public StreamLayer(double gradient = 0.01)
        {
            LayerQueue = new List<Layer>();
            Gradient = gradient;
        }

        public List<Layer> LayerQueue { set; get; }
        public double Gradient { get; }


        private DataShape InputDataShape { set; get; }
        private int OutputDataShape { set; get; }


        private void CreateFullConnectLayer(int[] neureList)
        {
            LayerQueue = new List<Layer>();
            var inputLayer = new LayerInput(InputDataShape) {LayerIndex = 0};
            LayerQueue.Add(inputLayer);
            neureList.ToList().ForEach(a =>
            {
                var dataShape = LayerQueue.Last().ShapeOut;
                var fullConnectedLayer = new LayerFullConnected(dataShape, a)
                {
                    LayerIndex = LayerQueue.Count
                };
                LayerQueue.Add(fullConnectedLayer);
            });

            var outputLayer = new LayerOutput(LayerQueue.Last().ShapeOut, OutputDataShape)
            {
                LayerIndex = LayerQueue.Count
            };
            LayerQueue.Add(outputLayer);
        }




        public void UpdateWeight(List<StreamData> streamDatas)
        {
            LayerQueue.ToList().ForEach(layer => { layer.UpdateNeure(); });
        }

        public void Serializer(string path)
        {
            using var fs = new FileStream(path, FileMode.Create);
            new XmlSerializer(typeof(StreamLayer)).Serialize(fs, this);
        }

        public static StreamLayer Deserialize(string path)
        {
            using var fs = new FileStream(path, FileMode.Open);
            return (StreamLayer) (new XmlSerializer(typeof(StreamLayer)).Deserialize(fs));
        }

    }

}
