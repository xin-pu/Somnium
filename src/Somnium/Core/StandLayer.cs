using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using MathNet.Numerics.LinearAlgebra.Double;


namespace Somnium.Core
{
    public abstract class StandLayer : ICloneable, INotifyPropertyChanged

    {
        private Matrix datasOutput;
        private Matrix datasInput;

        protected StandLayer(DataSize dataSize)
        {
            DataSizeInputFormat = dataSize;
        }
        public DataSize DataSizeInputFormat { get; }
        public DataSize DataSizeOutputFormat { get; }

        public int LayerColumnIndex { set; get; }
        public int LayerRowIndex { set; get; }

        public Matrix DatasInput
        {
            set { UpdateProperty(ref datasInput, value); }
            get { return datasInput; }
        }

        public Matrix DatasOutput
        {
            set { UpdateProperty(ref datasOutput, value); }
            get { return datasOutput; }
        }

        public virtual bool DatasCheckIn(Matrix datas)
        {
            var equal = datas.RowCount == DataSizeOutputFormat.RowCount && datas.ColumnCount == DataSizeOutputFormat.ColumnCount;
            if (equal)
                DatasInput = datas;
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
