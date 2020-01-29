﻿using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    [Serializable]
    public class LayerFullConnected : Layer
    {

        public int NeureCount { protected set; get; }

        public NeurePerceptron[] Perceptrons { set; get; }

        public LayerFullConnected(DataShape shape, int neureCount,int layerIndex) : base(shape,layerIndex)
        {
            NeureCount = neureCount;
            Perceptrons = Enumerable.Range(0, neureCount)
                .Select(a => new NeurePerceptron(shape.Levels, 1)
                {
                    LayerIndex = layerIndex,
                    Order = a
                })
                .ToArray();
        }


        //public override bool CheckInData(Matrix[] DataIn)
        //{
        //    var dataIn = dataFlow.Data;
        //    var columns = dataIn.Select(a => a.ColumnCount).Distinct().ToArray();
        //    var rows= dataIn.Select(a => a.RowCount).Distinct().ToArray();
        //    if (columns.Length != 1 || rows.Length != 1) return false;
        //    var equal = dataIn.Length == ShapeIn.Layers &&
        //                rows.First() == ShapeIn.Rows &&
        //                columns.First() == ShapeIn.Columns;
        //    if (equal)
        //    {
        //       var DataIn = DimensionalityReduction(dataIn);
        //    }

        //    return equal;
        //}


        private Matrix DimensionalityReduction(Matrix[] datas)
        {
            var datasArrays = datas.Select(a => a.Enumerate());
            var datasOutput = new List<double>();
            datasArrays.ToList().ForEach(a => datasOutput.AddRange(a));
            return new DenseMatrix(datasOutput.Count, 1, datasOutput.ToArray());
        }


        public override Array Activated(DataFlow dataFlow, Matrix datas)
        {
            throw new NotImplementedException();
        }

        public override Array Activated(DataFlow dataFlow, Array datas)
        {
            throw new NotImplementedException();
        }
    }
}