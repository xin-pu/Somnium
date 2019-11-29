using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using MathNet.Numerics.LinearAlgebra.Double;


namespace Somnium.Core
{

    [Serializable]
    public abstract class StandLayer : ICloneable, INotifyPropertyChanged

    {
        private List<Matrix> _outputDatas;
        private List<Matrix> _inputDatas;

        protected StandLayer()
        {

        }

        protected StandLayer(DataSize inputDataSize)
        {
            InputDataSizeFormat = inputDataSize;
        }

        public DataSize InputDataSizeFormat { get;  set; }
        public DataSize OutputDataSizeFormat { get; set; }

        public int LayerColumnIndex { set; get; }
        public int LayerRowIndex { set; get; }

        [XmlIgnore]
        public List<Matrix> InputDatas
        {
            set => UpdateProperty(ref _inputDatas, value);
            get => _inputDatas;
        }
        [XmlIgnore]
        public List<Matrix> OutputDatas
        {
            set => UpdateProperty(ref _outputDatas, value);
            get => _outputDatas;
        }

        public abstract void Save(string path);


        public object Clone()
        {
            var bf = new XmlSerializer(GetType());
            var memStream = new MemoryStream();
            bf.Serialize(memStream, this);
            memStream.Flush();
            memStream.Position = 0;
            return bf.Deserialize(memStream);
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
