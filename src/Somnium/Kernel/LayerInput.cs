using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    /// <summary>
    /// Input Layer is the first Layer which the input size and output size are same.
    /// So Activated method will return the input data.
    /// </summary>
    [Serializable]
    public class LayerInput : Layer
    {

        public LayerInput()
        {
        }
 

        public LayerInput(DataShape shape)
            : base(shape)
        {

        }


        public override Tuple<Matrix, Matrix> Activated(Matrix datas)
        {
            return new Tuple<Matrix, Matrix>(datas, datas);
        }

        public override void Deviated(StreamData data, double gradient)
        {

        }

        public override void UpdateNeure()
        {

        }

        public override void Serializer(string filename)
        {
            using var fs = new FileStream(filename, FileMode.Create);
            new XmlSerializer(typeof(LayerInput)).Serialize(fs, this);
        }
    }
}
