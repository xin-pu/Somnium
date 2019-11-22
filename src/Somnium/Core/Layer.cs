using System;
using System.IO;
using System.Xml.Serialization;
using MathNet.Numerics.LinearAlgebra;


namespace Somnium.Core
{
    public abstract class Layer<T> : ICloneable
        where T : struct, IEquatable<T>, IFormattable
    {
        private Matrix<T> datasOutput;

        public string Name { set; get; }

        public DataSize DataSize { set; get; }

        public int LayerColumnIndex { set; get; }
        public int LayerRowIndex { set; get; }

        public Matrix<T> DatasOutput
        {
            set
            {
                datasOutput = value;
                DataSize = new DataSize
                {
                    ColumnCount = value.ColumnCount,
                    RowCount = value.RowCount
                };
            }
            get { return datasOutput; }
        }


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
