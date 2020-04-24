﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Somnium.Kernel;

namespace Somnium.Train
{
    public abstract class StandTrain : ICloneable, INotifyPropertyChanged
    {

        private string _name;
        private DateTime _createDateTime;
        private DateTime _startTime;
        private DateTime _stopTime;
        private TimeSpan _timeSpan;
        private double _learningRate = 0.1;
        private uint _trainCountLimit = 1000;
        private uint _trainCountCurrent;
        private DataShape _dataShapeIn;
        private double _correctRateCurrent;
        private List<double> _correctRates;


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
        protected StandTrain()
        {
            CreateTime = DateTime.Now;
            Name = $"Train_{CreateTime:hh_mm_ss}";
        }

        public virtual void SetDataShapeInFormat(int rows, int columns)
        {
            DataShapeIn = new DataShape(rows, columns);
        }

        public abstract void ExecuteTrain(StreamLayer layerNet, TrainDataManager trainDataManager);



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