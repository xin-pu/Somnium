using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using Somnium.Core;

namespace Somnium.Learner
{
    public class DeepLearner : ICloneable, INotifyPropertyChanged
    {
        private TrainParameters _trainParameters;
        private LayerNetManager _layerNetManager;
        private string _name;
        private DateTime _createDateTime;
        private DateTime _startTime;
        private DateTime _stopTime;
        private TimeSpan _timeSpan;
        private double _correctRateCurrent;
        private List<double> _correctRates;
        private uint _trainCountCurrent;

        
        public TrainParameters TrainParameters
        {
            set => UpdateProperty(ref _trainParameters, value);
            get => _trainParameters;
        }


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


        public uint TrainCountCurrent
        {
            set => UpdateProperty(ref _trainCountCurrent, value);
            get => _trainCountCurrent;
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

        public LayerNetManager LayerNetManager
        {
            set => UpdateProperty(ref _layerNetManager, value);
            get => _layerNetManager;
        }

    
        /// <summary>
        /// We will Create a Train with Default Value
        /// </summary>
        public DeepLearner(TrainDataManager trainDataManager, TrainParameters trainParameters)
        {
            LayerNetManager = new LayerNetManager(trainDataManager, trainParameters); 
            TrainParameters = trainParameters;
            CreateTime = DateTime.Now;
            Name = $"Train_1";
        }


        public virtual void ExecuteTrain(TrainDataManager trainDataManager)
        {
            var inputStreams = trainDataManager.StreamDatas;
            StartTime = DateTime.Now;
            TrainCountCurrent = 0;
            CorrectRates = new List<double>();

            for (TrainCountCurrent = 1; TrainCountCurrent <= TrainParameters.TrainCountLimit; TrainCountCurrent++)
            {
                //以神经网络层更新数据层
                inputStreams.AsParallel().ForAll(singleStream => singleStream.ActivateLayerNet(LayerNetManager));

                CorrectRateCurrent = inputStreams.Count(a => a.IsMeetExpect) * 1.0 / inputStreams.Count;
                CorrectRates.Add(CorrectRateCurrent);
                Console.WriteLine($"当前训练次数：{TrainCountCurrent}\t\t准确率：{CorrectRateCurrent:P}");

                //反向传播误差
                inputStreams.AsParallel().ForAll(singleStream => singleStream.ErrorBackPropagation(LayerNetManager));

                //从数据层收集误差并更新神经网络层的神经元
                LayerNetManager.UpdateWeight();
                LayerNetManager.Serializer("D:\\test.xml");
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
