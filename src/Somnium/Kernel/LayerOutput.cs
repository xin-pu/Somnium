using System;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    public class LayerOutput:Layer
    {

        public Matrix DataIn { set; get; }

        public Matrix DataOut { set; get; }

        

      

        public LayerOutput(int rows, int columns, int layers, int layIndex) : base(rows, columns, layers, layIndex)
        {
        }

        public LayerOutput(DataShape shape, int layerIndex) : base(shape, layerIndex)
        {
        }

        public override Array Activated(DataFlow dataFlow, Matrix datas)
        {
            throw new NotImplementedException();
        }

        public override Array Activated(DataFlow dataFlow, Array datas)
        {
            throw new NotImplementedException();
        }


        //public override bool CheckInData(DataFlow dataFlow)
        //{
        //    var dataIn = dataFlow.Data;
        //    var columns = dataIn.Select(a => a.ColumnCount).Distinct().ToArray();
        //    var rows = dataIn.Select(a => a.RowCount).Distinct().ToArray();
        //    if (columns.Length != 1 || rows.Length != 1) return false;
        //    var equal = dataIn.Length == ShapeIn.Layers &&
        //                rows.First() == ShapeIn.Rows &&
        //                columns.First() == ShapeIn.Columns;
        //    if (equal)
        //    {

        //    }

        //    return equal;
        //}
    }
}
