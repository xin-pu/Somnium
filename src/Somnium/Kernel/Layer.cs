using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    public abstract class Layer : ICloneable
    {
        public int LayerIndex { protected set; get; }
        public DataShape ShapeIn { protected set; get; }
        public DataShape ShapeOut { protected set; get; }


        protected Layer(int rows, int columns, int layers)
        {
            var degreeIn = new DataShape {Rows = rows, Columns = columns, Layers = layers};
            ShapeIn = ShapeOut = degreeIn;
        }

        protected Layer(DataShape shape)
        {
            ShapeIn = ShapeOut = shape;
        }

        public virtual void Save(string path)
        {
            using var fs = new FileStream(path, FileMode.Create);
            var serializer = new BinaryFormatter();
            serializer.Serialize(fs, this);
        }

        public abstract Array Activated(StreamLayer layerNet, Matrix datas);
        public abstract Array Activated(StreamLayer layerNet, Array datas);
        
        public object Clone()
        {
            var serializer = new BinaryFormatter();
            var memStream = new MemoryStream();
            serializer.Serialize(memStream, this);
            memStream.Flush();
            memStream.Position = 0;
            return serializer.Deserialize(memStream);
        }
    }
}
