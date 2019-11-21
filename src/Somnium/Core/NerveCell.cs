using MathNet.Numerics.LinearAlgebra;
using System;
using System.IO;
using System.Xml.Serialization;


namespace Somnium.Core
{
    [Serializable]
    public abstract class NerveCell<T> : ICloneable
        where T : struct, IEquatable<T>, IFormattable
    {


        public Matrix<T> Weight { set; get; }

        public double Bias { set; get; }

        public Matrix<T> DeltaWeight { set; get; }

        public double DeltaBias { set; get; }

        public abstract Func<T, double> ActivateFuc { set; get; }

        public abstract Func<T, double> DeltaActivateFuc { set; get; }

        public int RowCount => Weight.RowCount;
        public int ColumnCount => Weight.ColumnCount;
        public int InputLevel=> Weight.RowCount * Weight.ColumnCount;
        public int OutputLevel { set; get; }

        public abstract double ActivateNerveCell(Layer<T> inputLayer);


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
