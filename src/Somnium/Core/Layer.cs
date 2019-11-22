using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using MathNet.Numerics.LinearAlgebra;


namespace Somnium.Core
{
    public abstract class Layer<T> : ICloneable ,INotifyPropertyChanged
        where T : struct, IEquatable<T>, IFormattable
    {
        private Matrix<T> datasOutput;


        protected Layer(DataSize dataSize)
        {
            DataSizeOutput = DataSizeInput = dataSize;
        }

        public DataSize DataSizeInput { get; }
        public DataSize DataSizeOutput { get; }

        public int LayerColumnIndex { set; get; }
        public int LayerRowIndex { set; get; }

        public Matrix<T> DatasInput
        {
            set { UpdateProperty(ref datasOutput, value); }
            get { return datasOutput; }
        }

        public Matrix<T> DatasOutput
        {
            set { UpdateProperty(ref datasOutput, value); }
            get { return datasOutput; }
        }

        public bool LoadDatasOutput(Matrix<T> datas)
        {
            var equal = datas.RowCount == DataSizeOutput.RowCount && datas.ColumnCount == DataSizeOutput.ColumnCount;
            if (equal)
                DatasOutput = datas;
            return equal;
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

        public void UpdateProperty<T>(ref T properValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (Equals(properValue, newValue))
            {
                return;
            }

            properValue = newValue;

            OnPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
