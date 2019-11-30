using System;
using System.IO;
using System.Xml.Serialization;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
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


        public object Clone()
        {
            var bf = new XmlSerializer(GetType());
            var memStream = new MemoryStream();
            bf.Serialize(memStream, this);
            memStream.Flush();
            memStream.Position = 0;
            return bf.Deserialize(memStream);
        }

        private void Initial(NeureShape shape)
        {
            Weight = DenseMatrix.CreateRandom(shape.Rows, shape.Columns,new Normal()).Values;
            Offset = new ContinuousUniform().Median;
        }
    }



}
