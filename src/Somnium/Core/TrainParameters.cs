using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Somnium.Func;
using Somnium.Kernel;
using Somnium.Utility;

namespace Somnium.Core
{

    public class TrainParameters : INotifyPropertyChanged, ICloneable
    {
        #region Function

        public Func<IEnumerable<double>, IEnumerable<double>, double> GetCost;
        public Func<IEnumerable<double>, IEnumerable<double>> GetLikelihood;

        #endregion


        private double _learningRate = 0.1;
        private int _trainCountLimit = 1000;
        private CostType _costType = CostType.Basic;
        private LikeliHoodType _likeliHoodType = LikeliHoodType.SoftMax;
        private AsyncObservableCollection<LayerStruct> _layerStructs = new AsyncObservableCollection<LayerStruct>();

        public double LearningRate
        {
            set => UpdateProperty(ref _learningRate, value);
            get => _learningRate;
        }

        public int TrainCountLimit
        {
            set => UpdateProperty(ref _trainCountLimit, value);
            get => _trainCountLimit;
        }

        public CostType CostType
        {
            set
            {
                UpdateProperty(ref _costType, value);
                switch (value)
                {
                    default:
                        GetCost = Cost.GetVariance;
                        break;
                }
            }
            get => _costType;
        }

        public LikeliHoodType LikeliHoodType
        {
            set
            {
                UpdateProperty(ref _likeliHoodType, value);
                switch (value)
                {
                    default:
                        GetLikelihood = LikeliHood.SoftMax;
                        break;
                }
            }
            get => _likeliHoodType;
        }


        public AsyncObservableCollection<LayerStruct> InterLayerStructs
        {
            protected set => UpdateProperty(ref _layerStructs, value);
            get => _layerStructs;
        }

        public void AppendDefaultLayer()
        {
            AddLayer(LayerType.FullConnectLayer, 10);
        }

        public void ClearLayer()
        {
            InterLayerStructs
                .Where(a => a.Selected).ToList()
                .ForEach(a => InterLayerStructs.RemoveAt(a.Index));
        }

        public void AddLayer(LayerType layerType, int neureCount)
        {
            var indexes = InterLayerStructs.Select(a => a.Index).ToArray();
            var newLayerStruct = new LayerStruct(layerType, neureCount)
            {
                Index = !indexes.Any() ? 0 : indexes.Max() + 1
            };
            InterLayerStructs.Add(newLayerStruct);
        }

        public void DeleteLayer()
        {

        }

        public object Clone()
        {
            return new TrainParameters
            {
                LearningRate = LearningRate,
                TrainCountLimit = TrainCountLimit,
                CostType = CostType,
                LikeliHoodType = LikeliHoodType,
                InterLayerStructs = InterLayerStructs
            };
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
