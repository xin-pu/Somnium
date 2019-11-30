using System;
using System.IO;
using System.Xml.Serialization;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    public abstract class Layer : ICloneable
    {
        public int NetIndex { set; get; }

        public DataDegree DegreeIn { set; get; }
        public DataDegree DegreeOut { set; get; }

        [XmlIgnore]
        public DataFlow DataIn { set; get; }
        [XmlIgnore]
        public DataFlow DataOut { set; get; }


        public abstract void Save(string path);
        public abstract bool CheckInData(DataFlow dataFlow);
        public abstract DataFlow CheckOutData();


        public object Clone()
        {
            var bf = new XmlSerializer(GetType());
            var memStream = new MemoryStream();
            bf.Serialize(memStream, this);
            memStream.Flush();
            memStream.Position = 0;
            return bf.Deserialize(memStream);
        }
    }
}
