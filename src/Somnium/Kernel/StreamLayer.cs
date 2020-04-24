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

        public StreamLayer(double gradient = 0.01)
        {
            LayerQueue = new List<Layer>();
            Gradient = gradient;
        }

        public List<Layer> LayerQueue { set; get; }
        public double Gradient { get; }


        public bool AddInputLayer(LayerInput layer)
        {
            if (LayerQueue.Count != 0) return false;
            layer.LayerIndex = 0;
            LayerQueue.Add(layer);
            return true;
        }

        public bool AddFullConnectedLayer(int neureCount)
        {
            var dataShape = LayerQueue.Last().ShapeOut;
            var fullConnectedLayer = new LayerFullConnected(dataShape, neureCount)
            {
                LayerIndex = LayerQueue.Count
            };
            LayerQueue.Add(fullConnectedLayer);
            return true;
        }

        public bool AddOutputLayer(int neureCount)
        {
            var dataShape = LayerQueue.Last().ShapeOut;
            var outputLayer = new LayerOutput(dataShape, neureCount)
            {
                LayerIndex = LayerQueue.Count
            };
            LayerQueue.Add(outputLayer);
            return true;
        }

        public void ClearLayer()
        {

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
