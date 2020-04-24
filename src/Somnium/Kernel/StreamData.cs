using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Func;

namespace Somnium.Kernel
{

    /// <summary>
    /// 用于存储单个样列 产生的所有的中间数据
    /// 必须数显从网络层神经元 复制的 delta数据
    /// </summary>
    public class StreamData
    {

        private Matrix _matrix;

        public StreamData()
        {
            LayerDatas=new Dictionary<int, LayerData>();
        }


        public Matrix InputDataMatrix
        {
            set
            {
                _matrix = value;
                InputDataShape = new DataShape(value.RowCount,value.ColumnCount);
            }
            get => _matrix;
        }

        public DataShape InputDataShape { set; get; }

        public Dictionary<int,LayerData> LayerDatas { set; get; }


        public bool IsMeetExpect => EstimateLabel.Equals(ExpectedLabel);
        public double[] EstimateOut { set; get; }
        public string EstimateLabel { set; get; }
        public double[] ExpectedOut { set; get; }
        public string ExpectedLabel { set; get; }
        public double SquareError { set; get; }

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
            EstimateOut = LayerDatas[LayerDatas.Count - 1].Activated.AsColumnMajorArray(); ;
            EstimateLabel = GetEstimateLabel.Invoke(GetLikelihoodRatio(EstimateOut));
            SquareError = GetCost.Invoke(ExpectedOut, EstimateOut);
        }

        public void ErrorBackPropagation(StreamLayer streamLayer, double gar)
        {
            streamLayer.LayerQueue.OrderByDescending(a => a.LayerIndex).ToList().ForEach(layer =>
            {
                layer.Deviated(this, gar);
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
            var res= SoftMax.BasicSoftMax(outputData).ToArray();
            return res.ToList().IndexOf(res.Max());
        }

        public static Func<string,StreamData> GetStreamData;
        public static Func<IEnumerable<double>, IEnumerable<double>, double> GetCost;
        public static Func<int, string> GetEstimateLabel;

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
    }


    public class LayerData
    {
        public Matrix Weighted { set; get; }
        public Matrix Activated { set; get; }
        public IEnumerable<double> Error { set; get; }
        public IEnumerable<double> SWd { set; get; }

    }


}
