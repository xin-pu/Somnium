using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Func;

namespace Somnium.Kernel
{

    /// <summary>
    /// 用于存储单个样列 产生的所有的中间数据
    /// 必须数显从网络层神经元 复制的 delta数据
    /// </summary>
    public class StreamData : INotifyPropertyChanged
    {

        public static Func<string, StreamData> GetStreamData;
        public static Func<IEnumerable<double>, IEnumerable<double>, double> GetCost;
        public static Func<IEnumerable<double>, IEnumerable<double>> GetLikelihood;
        public static Func<int, string> GetEstimateLabel;


        public static CostType CostType
        {
            set
            {
                _costType = value;
                switch (value)
                {
                    default:
                        GetCost = Cost.GetVariance;
                        break;
                }
            }
            get => _costType;
        }
        public static LikeliHoodType LikeliHoodType
        {
            set
            {
                _likeliHoodType = value;
                switch (value)
                {
                    default:
                        GetLikelihood = LikeliHood.SoftMax;
                        break;
                }
            }
            get => _likeliHoodType;
        }
        private static CostType _costType = CostType.Basic;
        private static LikeliHoodType _likeliHoodType = LikeliHoodType.SoftMax;


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



        public StreamData()
        {
            LayerDatas = new Dictionary<int, LayerData>();
        }


        public void ActivateLayerNet(StreamLayer streamLayer)
        {
            var tempData = InputDataMatrix;
            streamLayer.LayerQueue.ToList().ForEach(layer =>
            {
                var (item1, item2) = layer.Activated(tempData);
                LayerDatas[layer.LayerIndex] = new LayerData
                {
                    Activated = item1,
                    Weighted = item2
                };
                tempData = item1;
            });
            EstimateOut = LayerDatas[LayerDatas.Count - 1].Activated.AsColumnMajorArray();
            EstimateLabel = GetEstimateLabel.Invoke(GetLikelihoodRatio(EstimateOut));
            SquareError = GetCost.Invoke(ExpectedOut, EstimateOut);
        }

        public void ErrorBackPropagation(StreamLayer streamLayer)
        {
            streamLayer.LayerQueue.OrderByDescending(a => a.LayerIndex).ToList().ForEach(layer =>
            {
                layer.Deviated(this, streamLayer.Gradient);
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

        public static StreamData CreateStreamData(string path)
        {
            return GetStreamData?.Invoke(path);
        }

        public static DataShape FilterDataShape(IEnumerable<StreamData> streamDatas)
        {
            var dataShapes = streamDatas.Select(a => a.InputDataShape).ToArray();
            if (dataShapes.Select(a => a.Rows).Distinct().Count() > 1 ||
                dataShapes.Select(a => a.Columns).Distinct().Count() > 1)
                throw new Exception();
            return dataShapes.First();
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
