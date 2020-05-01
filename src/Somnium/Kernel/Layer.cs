using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Core;

namespace Somnium.Kernel
{
    [Serializable]
    public abstract class Layer : ICloneable, INotifyPropertyChanged
    {
        private int _layIndex;
        private DataShape _shapeIn;
        private DataShape _shapeOut;

        public int LayerIndex
        {
            set => UpdateProperty(ref _layIndex, value);
            get => _layIndex;
        }

        public DataShape ShapeIn
        {
            set => UpdateProperty(ref _shapeIn, value);
            get => _shapeIn;
        }

        public DataShape ShapeOut
        {
            set => UpdateProperty(ref _shapeOut, value);
            get => _shapeOut;
        }

        protected Layer()
        {
        }

        protected Layer(int rows, int columns, int layers)
        {
            var degreeIn = new DataShape {Rows = rows, Columns = columns, Layers = layers};
            ShapeIn = ShapeOut = degreeIn;
        }

        protected Layer(DataShape shape)
        {
            ShapeIn = ShapeOut = shape;
        }


        public abstract Tuple<Matrix, Matrix> Activated(Matrix datas);
        public abstract void Deviated(StreamData data, double gradient);
        public abstract void UpdateNeure();
        public abstract void Serializer(string filename);

        public object Clone()
        {
            var serializer = new BinaryFormatter();
            var memStream = new MemoryStream();
            serializer.Serialize(memStream, this);
            memStream.Flush();
            memStream.Position = 0;
            return serializer.Deserialize(memStream);
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }


    public class LayerStruct
    {
        public LayerStruct(LayerType layerType, int neureCount)
        {
            LayerType = layerType;
            NeureCount = neureCount;
            Selected = true;
        }

        public bool Selected { set; get; }
        public LayerType LayerType { set; get; }
        public int NeureCount { set; get; }
        public int Index { set; get; }

    }

    public enum LayerType
    {
        FullConnectLayer,
        EmptyLayer
    }
}
