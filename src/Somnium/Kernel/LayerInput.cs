using System;
using System.Linq;

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
            var degreeIn = new DataDegree {Rows = rows, Columns = columns, Layers = layers};
            DegreeIn = DegreeOut = degreeIn;
        }

        public LayerInput(DataDegree degree)
        {
            DegreeIn = DegreeOut = degree;
        }


        public override void Save(string path)
        {
            throw new NotImplementedException();
        }

        public override bool CheckInData(DataFlow dataFlow)
        {
            var dataIn = dataFlow.InputData;
            if (dataIn.Length != DegreeIn.Layers) return false;
            var data = dataIn.First();
            var equal = data.RowCount == DegreeIn.Rows
                        && data.ColumnCount == DegreeIn.Columns;
            if (equal)
            {
                DataIn = dataFlow;
            }

            return equal;
        }

        public override DataFlow CheckOutData()
        {
            DataOut = DataIn;
            return DataOut;
        }
    }
}
