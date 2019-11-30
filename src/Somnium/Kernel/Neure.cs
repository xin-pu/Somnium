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
        public uint Order { set; get; }
        public NeureDegree Degree { set; get; }
        public double[] Weight { set; get; }
        public double Offset { set; get; }

        protected Neure()
        {

        }

        protected Neure(NeureDegree degree)
        {
            Initial(degree);
        }

        protected Neure(int rows, int columns)
        {
            Initial(new NeureDegree {Rows = rows, Columns = columns});
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

        private void Initial(NeureDegree degree)
        {
            Weight = DenseMatrix.CreateRandom(degree.Rows, degree.Columns, new ContinuousUniform()).Values;
            Offset = new ContinuousUniform().Median;
        }
    }



}
