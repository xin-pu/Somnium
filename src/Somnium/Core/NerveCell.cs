using MathNet.Numerics.LinearAlgebra;
using System;
using System.IO;
using System.Xml.Serialization;


namespace Somnium.Core
{

    /// <summary>
    /// Nervecell is a abstract class which fit different operate  
    /// It Need dataSize to check the input data is corrret
    /// It will Create a datasize according the different operate
    /// NerverCell will Ouput double data as example
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class NerveCell<T> : ICloneable
        where T : struct, IEquatable<T>, IFormattable
    {


        public Matrix<T> Weight { set; get; }

        public double Bias { set; get; }

        public Matrix<T> DeltaWeight { set; get; }

        public double DeltaBias { set; get; }

        
        public DataSize DataSize { set; get; }
        public DataSize OutputLevel { set; get; }

        protected NerveCell(int column, int row)
        {
            DataSize = new DataSize {ColumnCount = column, RowCount = row};
        }

        protected NerveCell(DataSize datasize)
        {
            DataSize = datasize;
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

    public struct DataSize
    {

        public int ColumnCount { set; get; }
        public int RowCount { set; get; }

        public int Level => ColumnCount * RowCount;
    }



}
