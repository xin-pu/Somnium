using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using JetBrains.Annotations;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    /// <summary>
    /// 基础神经元
    /// </summary>
    [Serializable]
    public abstract class Neure : ICloneable,INotifyPropertyChanged
    {
        [XmlIgnore]
        public readonly object MyLock = new object();

        private int _layIndex;
        private int _order;
        private NeureShape _shape;
        private Matrix _weight;
        private double[] _weightArray;
        private double _offset;

        public int LayerIndex
        {
            set => UpdateProperty(ref _layIndex, value);
            get => _layIndex;
        }
        public int Order
        {
            set => UpdateProperty(ref _order, value);
            get => _order;
        }
        public NeureShape Shape
        {
            set => UpdateProperty(ref _shape, value);
            get => _shape;
        }

        [XmlIgnore]
        public Matrix Weight
        {
            set
            {
                UpdateProperty(ref _weight, value);
                _weightArray = value.AsColumnMajorArray();
            }
            get => _weight;
        }

        public double[] WeightArray
        {
            set
            {
                UpdateProperty(ref _weightArray, value);
                _weight = DenseMatrix.OfColumnMajor(Shape.Rows, Shape.Columns, value);
            }
            get => _weightArray;
        }

        public double Offset
        {
            set => UpdateProperty(ref _offset, value);
            get => _offset;
        }

        [XmlIgnore]
        public Matrix WeightDelta { set; get; }
        [XmlIgnore]
        public double OffsetDelta { set; get; }

        protected Neure()
        {

        }

        protected Neure(NeureShape shape)
        {
            Initial(shape);
        }

        protected Neure(int rows, int columns)
        {
            var shape = new NeureShape {Rows = rows, Columns = columns};
            Initial(shape);
        }


        public abstract Tuple<double, double> Activated(Matrix inputData);
        public abstract void AddDeviation(Matrix devWeight, double devBias);
        public abstract void UpdateDeviation();

        public virtual object Clone()
        {
            var serializer = new BinaryFormatter();
            var memStream = new MemoryStream();
            serializer.Serialize(memStream, this);
            memStream.Flush();
            memStream.Position = 0;
            return serializer.Deserialize(memStream);
        }

        private void Initial(NeureShape shape)
        {
            Weight = DenseMatrix.CreateRandom(shape.Rows, shape.Columns, new Normal());
            Offset = new ContinuousUniform().Median;

            WeightDelta = DenseMatrix.Create(shape.Rows, shape.Columns, 0);
            OffsetDelta = 0;
            Shape = shape;
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

}
