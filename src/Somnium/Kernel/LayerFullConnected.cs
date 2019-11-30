using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Somnium.Core;

namespace Somnium.Kernel
{
    public class LayerFullConnected:Layer
    {

        public int NeureCount { set; get; }
        public Perceptron[] Perceptrons { set; get; }

        public LayerFullConnected(DataDegree degree, int neureCount)
        {
            DegreeIn = degree;
            DegreeOut = new DataDegree() {Rows = 1, Columns = neureCount, Layers = 1};
            Perceptrons = Enumerable.Range(0, neureCount)
                .Select(a => new Perceptron(degree.Levels, 1))
                .ToArray();

        }

        public override void Save(string path)
        {
            using var fs = new FileStream(path, FileMode.Create);
            var formatter = new XmlSerializer(typeof(FullConnectedLayer));
            formatter.Serialize(fs, this);
        }

        public override bool CheckInData(DataFlow dataFlow)
        {
            throw new NotImplementedException();
        }

        public override DataFlow CheckOutData()
        {
            throw new NotImplementedException();
        }
    }
}
