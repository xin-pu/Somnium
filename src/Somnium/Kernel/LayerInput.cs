using System;
using System.Linq;
using System.Xml.Serialization;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    [Serializable]
    public class LayerInput : Layer
    {


        public LayerInput()
        {

        }


        public LayerInput(int rows, int columns, int layers)
        {
            var degreeIn = new DataShape {Rows = rows, Columns = columns, Layers = layers};
            ShapeIn = ShapeOut = degreeIn;
        }

        public LayerInput(DataShape shape)
        {
            ShapeIn = ShapeOut = shape;
        }


        public override void Save(string path)
        {
            throw new NotImplementedException();
        }

        public override bool CheckInData(DataFlow dataFlow)
        {
            var dataIn = dataFlow.Data;
            if (dataIn.Length != ShapeIn.Layers) return false;
            var data = dataIn.First();
            var equal = data.RowCount == ShapeIn.Rows
                        && data.ColumnCount == ShapeIn.Columns;
            return equal;
        }


    }
}
