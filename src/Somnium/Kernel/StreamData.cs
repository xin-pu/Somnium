using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using DenseMatrix = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix;

namespace Somnium.Kernel
{

    /// <summary>
    /// 用于存储单个样列 产生的所有的中间数据
    /// </summary>
    public class StreamData
    {

        private Matrix _matrix;

        public StreamData()
        {
            
        }

        public double[] InPutData { set; get; }

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
        public List<Matrix> QueueData { set; get; }


        public double[] ExpectedOut { set; get; }
        public double[] ActualOut { set; get; }
        public string ExpectedLabel { set; get; }
        public string ActualLabel { set; get; }



        public static Func<string,StreamData> GetStreamData;

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


    public class DataUnit<T>
    {
        public DataFormat Format { set; get; }
        public T DataValue { set; get; }

    }

    public enum DataFormat
    {
        MatrixArray,
        Matrix,
        DoubleArray
    }
}
