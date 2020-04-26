using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Somnium.Func;

namespace Somnium.Core
{
    public class TrainParameters : INotifyPropertyChanged
    {


        #region Function

        public Func<IEnumerable<double>, IEnumerable<double>, double> GetCost;
        public Func<IEnumerable<double>, IEnumerable<double>> GetLikelihood;

        #endregion


        private double _learningRate = 0.1;
        private int _trainCountLimit = 1000;
        private CostType _costType = CostType.Basic;
        private LikeliHoodType _likeliHoodType = LikeliHoodType.SoftMax;


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


        public TrainParameters()
        {

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
