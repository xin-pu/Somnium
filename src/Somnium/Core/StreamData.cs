﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Kernel;

namespace Somnium.Core
{

    /// <summary>
    /// 用于存储单个样列 产生的所有的中间数据
    /// 必须数显从网络层神经元 复制的 delta数据
    /// </summary>
    public class StreamData : INotifyPropertyChanged,ICloneable
    {

        public Func<IEnumerable<double>, IEnumerable<double>, double> GetCost;
        public Func<IEnumerable<double>, IEnumerable<double>> GetLikelihood;
        public Func<int, string> GetEstimateLabel;


        private Matrix _matrix;
        private bool _isMeetExpect;
        private double[] _estimateOut;
        private string _estimateLabel;
        private double[] _expectedOut;
        private string _expectedLabel;
        private double _squareError;

        public Matrix InputDataMatrix
        {
            set
            {
                _matrix = value;
                InputDataShape = new DataShape(value.RowCount, value.ColumnCount);
            }
            get => _matrix;
        }

        public DataShape InputDataShape { protected set; get; }
        public Dictionary<int, LayerData> LayerDatas { set; get; }

        public bool IsMeetExpect
        {
            set => UpdateProperty(ref _isMeetExpect, value);
            get => _isMeetExpect;
        }

        public double[] EstimateOut
        {
            set => UpdateProperty(ref _estimateOut, value);
            get => _estimateOut;
        }

        public string EstimateLabel
        {
            set
            {
                UpdateProperty(ref _estimateLabel, value);
                IsMeetExpect = EstimateLabel.Equals(ExpectedLabel);
            }
            get => _estimateLabel;
        }

        public double[] ExpectedOut
        {
            set => UpdateProperty(ref _expectedOut, value);
            get => _expectedOut;
        }

        public string ExpectedLabel
        {
            set => UpdateProperty(ref _expectedLabel, value);
            get => _expectedLabel;
        }

        public double SquareError
        {
            set => UpdateProperty(ref _squareError, value);
            get => _squareError;
        }


        public StreamData(Matrix matrixData, string actual)
        {
            LayerDatas = new Dictionary<int, LayerData>();
            InputDataMatrix = matrixData;
            ExpectedLabel = actual;
        }

        public void ActivateLayerNet(LayerNetManager layerNet)
        {
            var tempData = InputDataMatrix;
            layerNet.LayerNet.ToList().ForEach(layer =>
            {
                var (item1, item2) = layer.Activated(tempData);
                LayerDatas[layer.LayerIndex] = new LayerData
                {
                    Activated = item1,
                    Weighted = item2
                };
                tempData = item1;
            });
            EstimateOut = LayerDatas[layerNet.LayerNet.Count - 1].Activated.AsColumnMajorArray();
            EstimateLabel = GetEstimateLabel.Invoke(GetLikelihoodRatio(EstimateOut));
            SquareError = GetCost.Invoke(ExpectedOut, EstimateOut);
        }

        public void ErrorBackPropagation(LayerNetManager layerNet)
        {
            layerNet.LayerNet.OrderByDescending(a => a.LayerIndex).ToList().ForEach(layer =>
            {
                layer.Deviated(this, layerNet.LearningRate);
            });
        }

        public double[] GetActivatedArray(int layIndex)
        {
            return LayerDatas[layIndex].Activated.AsColumnMajorArray();
        }

        public Matrix GetActivatedMatrix(int layIndex)
        {
            return LayerDatas[layIndex].Activated;
        }


        public double[] GetError(int layIndex)
        {
            return LayerDatas[layIndex].Error.ToArray();
        }

        public double[] GetSwd(int layIndex)
        {
            var data = LayerDatas[layIndex].SWd;
            return data.ToArray();
        }

        private int GetLikelihoodRatio(IEnumerable<double> outputData)
        {
            var res = GetLikelihood.Invoke(outputData).ToArray();
            return res.ToList().IndexOf(res.Max());
        }

        public object Clone()
        {
            return new StreamData(InputDataMatrix, ExpectedLabel)
            {
                ExpectedOut = ExpectedOut,
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


    public class LayerData
    {
        public Matrix Weighted { set; get; }
        public Matrix Activated { set; get; }
        public IEnumerable<double> Error { set; get; }
        public IEnumerable<double> SWd { set; get; }

    }


}
