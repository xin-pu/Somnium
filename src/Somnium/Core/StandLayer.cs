using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using MathNet.Numerics.LinearAlgebra.Double;


namespace Somnium.Core
{
    public abstract class StandLayer : ICloneable, INotifyPropertyChanged

    {
        private IList<Matrix> _outputDatas;
        private IList<Matrix> _inputDatas;

        protected StandLayer(DataSize inputDataSize)
        {
            InputDataSizeFormat = inputDataSize;
        }

        public DataSize InputDataSizeFormat { get; protected set; }
        public DataSize OutputDataSizeFormat { get; protected set; }

        public int LayerColumnIndex { set; get; }
        public int LayerRowIndex { set; get; }

        public IList<Matrix> InputDatas
        {
            set => UpdateProperty(ref _inputDatas, value);
            get => _inputDatas;
        }

        public IList<Matrix> OutputDatas
        {
            set => UpdateProperty(ref _outputDatas, value);
            get => _outputDatas;
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


        #region

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

        #endregion

    }

}
