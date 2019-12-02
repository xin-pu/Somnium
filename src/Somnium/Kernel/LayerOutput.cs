using System.IO;
using System.Linq;
using System.Xml.Serialization;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Core;

namespace Somnium.Kernel
{
    public class LayerOutput:Layer
    {

        [XmlIgnore]
        public Matrix DataIn { set; get; }
        [XmlIgnore]
        public Matrix DataOut { set; get; }

        public override void Save(string path)
        {
            using var fs = new FileStream(path, FileMode.Create);
            var formatter = new XmlSerializer(typeof(FullConnectedLayer));
            formatter.Serialize(fs, this);
        }

        public override bool CheckInData(DataFlow dataFlow)
        {
            var dataIn = dataFlow.Data;
            var columns = dataIn.Select(a => a.ColumnCount).Distinct().ToArray();
            var rows = dataIn.Select(a => a.RowCount).Distinct().ToArray();
            if (columns.Length != 1 || rows.Length != 1) return false;
            var equal = dataIn.Length == ShapeIn.Layers &&
                        rows.First() == ShapeIn.Rows &&
                        columns.First() == ShapeIn.Columns;
            if (equal)
            {
              
            }

            return equal;
        }
    }
}
