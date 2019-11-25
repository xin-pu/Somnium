using System;
using System.IO;
using System.Xml.Serialization;
using MathNet.Numerics.LinearAlgebra.Double;


namespace Somnium.Core
{

    /// <summary>
    /// Nerve cell is a abstract class which fit different operate  
    /// It Need inputDataSize to check the input data is correct
    /// It will Create a data size according the different operate
    /// Nerve Cell will Output double data as example
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class NerveCell : ICloneable
    {


        public Matrix Weight { set; get; }

        public double Bias { set; get; }

        public Matrix DeltaWeight { set; get; }

        public double DeltaBias { set; get; }


        public DataSize DataSize { set; get; }
        public DataSize OutputLevel { set; get; }

        protected NerveCell(DataSize dataSize)
        {
            DataSize = dataSize;
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

    public class DataSize
    {

        public int ColumnCount { set; get; }
        public int RowCount { set; get; }
        public int DataCount { set; get; } = 1;
        public int Level => DataCount * ColumnCount * RowCount;
    }


}
