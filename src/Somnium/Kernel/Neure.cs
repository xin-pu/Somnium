using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    /// <summary>
    /// 基础神经元
    /// </summary>
    [Serializable]
    public abstract class Neure : ICloneable
    {

        public int LayerIndex { set; get; }
        public int Order { set; get; }
        public NeureShape Shape { set; get; }
        public double[] Weight { set; get; }
        public double Offset { set; get; }

        protected Neure()
        {

        }

        protected Neure(NeureShape shape)
        {
            Initial(shape);
        }

        protected Neure(int rows, int columns)
        {
            Initial(new NeureShape {Rows = rows, Columns = columns});
        }


        public virtual object Clone()
        {
            var serializer = new BinaryFormatter();
            var memStream = new MemoryStream();
            serializer.Serialize(memStream, this);
            memStream.Flush();
            memStream.Position = 0;
            return serializer.Deserialize(memStream);
        }

        private void Initial(NeureShape shape)
        {
            Weight = DenseMatrix.CreateRandom(shape.Rows, shape.Columns, new Normal()).Values;
            Offset = new ContinuousUniform().Median;
            Shape = shape;
        }
    }


}
