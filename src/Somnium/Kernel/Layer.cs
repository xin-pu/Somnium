using System;
using System.IO;
using System.Xml.Serialization;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    public abstract class Layer : ICloneable
    {
        public int LayerIndex { set; get; }

        public DataShape ShapeIn { set; get; }
        public DataShape ShapeOut { set; get; }



        public abstract void Save(string path);
        public abstract bool CheckInData(Array datas);

        public object Clone()
        {
            CheckInData(new[] { 1 });
            CheckInData(new[,] { { 1, 1 }, { 1, 1 } });
            var bf = new XmlSerializer(GetType());
            var memStream = new MemoryStream();
            bf.Serialize(memStream, this);
            memStream.Flush();
            memStream.Position = 0;
            return bf.Deserialize(memStream);
        }
    }
}
