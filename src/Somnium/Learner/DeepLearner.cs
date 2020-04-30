using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Input;
using JetBrains.Annotations;
using Somnium.Core;
using Somnium.Utility;

namespace Somnium.Learner
{
    public class DeepLearner : ICloneable, INotifyPropertyChanged
    {
        private TrainParameters _trainParameters;
        private TrainDataManager _trainDataManager;
        private LayerNetManager _layerNetManager;
        private string _name;
        private DateTime _createDateTime;
        private TimeSpan _costTime;
        private TimeSpan _remainTime;
        private double _learnSpeed;
        private double _correctRateCurrent;
        private List<double> _correctRates;
        private uint _trainCountCurrent;
        private bool _isExecuting = false;
        private Stopwatch _stopwatch;

        public ICommand ExecuteTrainCmd { set; get; }


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

        public Stopwatch Stopwatch
        {
            set => UpdateProperty(ref _stopwatch, value);
            get => _stopwatch;
        }

        public TimeSpan CostTime
        {
            set => UpdateProperty(ref _costTime, value);
            get => _costTime;
        }

        public TimeSpan RemainTime
        {
            set => UpdateProperty(ref _remainTime, value);
            get => _remainTime;
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

        public TrainDataManager TrainDataManager
        {
            set => UpdateProperty(ref _trainDataManager, value);
            get => _trainDataManager;
        }

        public bool IsExecuting
        {
            set => UpdateProperty(ref _isExecuting, value);
            get => _isExecuting;
        }

        public double LearnSpeed
        {
            set => UpdateProperty(ref _learnSpeed, value);
            get => _learnSpeed;
        }

        public ICommand TrainExecuteCmd { set; get; }



        /// <summary>
        /// We will Create a Train with Default Value
        /// </summary>
        public DeepLearner(TrainDataManager trainDataManager, TrainParameters trainParameters)
        {
            TrainDataManager = trainDataManager;
            TrainParameters = trainParameters;
            LayerNetManager = new LayerNetManager(trainDataManager, trainParameters);
            CreateTime = DateTime.Now;
            Name = $"Train_{CreateTime:HH_mm_SS}";
            TrainExecuteCmd = new RelayCommand(() => Task.Run(ExecuteTrain));
        }


        public virtual void ExecuteTrain()
        {
            if (IsExecuting)
            {
                IsExecuting = false;
            }
            else
            {
                var inputStreams = TrainDataManager.StreamDatas;
                LayerNetManager = new LayerNetManager(TrainDataManager, TrainParameters);
                Stopwatch = new Stopwatch();
                Stopwatch.Start();
                TrainCountCurrent = 0;
                CorrectRates = new List<double>();
                IsExecuting = true;
                for (TrainCountCurrent = 1; TrainCountCurrent <= TrainParameters.TrainCountLimit; TrainCountCurrent++)
                {
                    if (!IsExecuting)
                        break;

                    //以神经网络层更新数据层
                    inputStreams.AsParallel().ForAll(singleStream => singleStream.ActivateLayerNet(LayerNetManager));

                    CorrectRateCurrent =
                        Math.Round(inputStreams.Count(a => a.IsMeetExpect) * 100.0 / inputStreams.Count, 1);
                    CorrectRates.Add(CorrectRateCurrent);
                    Console.WriteLine($"当前训练次数：{TrainCountCurrent}\t\t准确率：{CorrectRateCurrent}");

                    //反向传播误差
                    inputStreams.AsParallel()
                        .ForAll(singleStream => singleStream.ErrorBackPropagation(LayerNetManager));

                    //从数据层收集误差并更新神经网络层的神经元
                    LayerNetManager.UpdateWeight();
                    LearnSpeed = Stopwatch.ElapsedMilliseconds * 1.0 / TrainCountCurrent;
                    CostTime = Stopwatch.Elapsed;
                    RemainTime =
                        new TimeSpan(
                            (long) ((TrainParameters.TrainCountLimit - TrainCountCurrent) * 10000 * LearnSpeed));

                }

                LayerNetManager.Serializer(Path.Combine(Environment.CurrentDirectory, "exp", $"{Name}.som"));
                Stopwatch.Stop();
                IsExecuting = false;
            }
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
