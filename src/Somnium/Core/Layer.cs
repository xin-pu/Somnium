using System;
using System.IO;
using System.Xml.Serialization;
using MathNet.Numerics.LinearAlgebra;


namespace Somnium.Core
{
    public abstract class Layer<T> : ICloneable
        where T : struct, IEquatable<T>, IFormattable
    {
        protected Layer(Matrix<T> datas)
        {
            DatasInput = datas;
        }

        public string Name { set; get; }
        public Matrix<T> DatasInput { set; get; }

        public int RowCount => DatasInput.RowCount;
        public int ColumnCount => DatasInput.ColumnCount;
        public int Levels => DatasInput.RowCount * DatasInput.ColumnCount;

        public object Clone()
        {
            var BF = new XmlSerializer(GetType());
            var memStream = new MemoryStream();
            BF.Serialize(memStream, this);
            memStream.Flush();
            memStream.Position = 0;
            return BF.Deserialize(memStream);
        }
    }

}
