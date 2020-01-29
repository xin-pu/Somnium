namespace Somnium.Core
{


    public class DataSize
    {

        public int ColumnCount { set; get; }
        public int RowCount { set; get; }
        public int DataCount { set; get; } = 1;
        public int Level => DataCount * ColumnCount * RowCount;
    }


}
