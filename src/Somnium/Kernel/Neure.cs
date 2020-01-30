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
        private readonly object _myLock = new object();

        public int LayerIndex { set; get; }
        public int Order { set; get; }
        public NeureShape Shape { set; get; }
        public Matrix Weight { set; get; }
        public double Offset { set; get; }
        public Matrix WeightMatrix { set; get; }

        protected Neure()
        {

        }

        protected Neure(NeureShape shape)
        {
            Initial(shape);
        }

        protected Neure(int rows, int columns)
        {
            var shape = new NeureShape {Rows = rows, Columns = columns};
            Initial(shape);
        }


        public abstract Tuple<double, double> Activated(Matrix inputData);
        public abstract Tuple<double, double> Updated();


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
            Weight = DenseMatrix.CreateRandom(shape.Rows, shape.Columns, new Normal());
            Offset = new ContinuousUniform().Median;
            Shape = shape;
        }
    }


}
