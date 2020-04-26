using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using Somnium.Core;
using Somnium.Kernel;

namespace Somnium.Train
{
    public class DeepLeaningModel : ICloneable, INotifyPropertyChanged
    {

        private string _name;

        private DateTime _createDateTime;
        private DateTime _startTime;
        private DateTime _stopTime;
        private TimeSpan _timeSpan;
        private double _correctRateCurrent;
        private List<double> _correctRates;
        private uint _trainCountLimit = 1000;
        private uint _trainCountCurrent;


        private double _learningRate = 0.1;
        private DataShape _dataShapeIn;
      
        
        public string Name
        {
            set => UpdateProperty(ref _name, value);
            get => _name;
        }

        public DateTime CreateTime
        {
            set => UpdateProperty(ref _createDateTime, value);
            get => _createDateTime;
        }

        public DateTime StartTime
        {
            set => UpdateProperty(ref _startTime, value);
            get => _startTime;
        }

        public DateTime StopTime
        {
            set => UpdateProperty(ref _stopTime, value);
            get => _stopTime;
        }

        public TimeSpan CostTime
        {
            set => UpdateProperty(ref _timeSpan, value);
            get => _timeSpan;
        }

        public double LearningRate
        {
            set => UpdateProperty(ref _learningRate, value);
            get => _learningRate;
        }

        public uint TrainCountLimit
        {
            set => UpdateProperty(ref _trainCountLimit, value);
            get => _trainCountLimit;
        }

        public uint TrainCountCurrent
        {
            set => UpdateProperty(ref _trainCountCurrent, value);
            get => _trainCountCurrent;
        }

        public DataShape DataShapeIn
        {
            set => UpdateProperty(ref _dataShapeIn, value);
            get => _dataShapeIn;
        }


        public double CorrectRateCurrent
        {
            set => UpdateProperty(ref _correctRateCurrent, value);
            get => _correctRateCurrent;
        }

        public List<double> CorrectRates
        {
            set => UpdateProperty(ref _correctRates, value);
            get => _correctRates;
        }

        /// <summary>
        /// We will Create a Train with Default Value
        /// </summary>
        public DeepLeaningModel()
        {
            CreateTime = DateTime.Now;
            Name = $"Train_{CreateTime:hh_mm_ss}";
        }

        public virtual void SetDataShapeInFormat(int rows, int columns)
        {
            DataShapeIn = new DataShape(rows, columns);
        }

        public virtual void ExecuteTrain(LayerNetManager layerNetNet, TrainDataManager trainDataManager)
        {
            var inputStreams = trainDataManager.StreamDatas;
            StartTime = DateTime.Now;
            TrainCountCurrent = 0;
            CorrectRates=new List<double>();

            for (TrainCountCurrent = 1; TrainCountCurrent <= TrainCountLimit; TrainCountCurrent++)
            {
                //以神经网络层更新数据层
                inputStreams.AsParallel().ForAll(singleStream => singleStream.ActivateLayerNet(layerNetNet));

                CorrectRateCurrent = inputStreams.Count(a => a.IsMeetExpect) * 100.0 / inputStreams.Count;
                CorrectRates.Add(CorrectRateCurrent);

                //反向传播误差
                inputStreams.AsParallel().ForAll(singleStream => singleStream.ErrorBackPropagation(layerNetNet));

                //从数据层收集误差并更新神经网络层的神经元
                layerNetNet.UpdateWeight();

            }

            StopTime = DateTime.Now;
        }


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


}
