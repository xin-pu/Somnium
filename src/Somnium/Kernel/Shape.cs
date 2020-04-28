using System;

namespace Somnium.Kernel
{
    public interface IShape
    {
        int Rows { set; get; }
        int Columns { set; get; }
        int Levels { get; }
    }


    [Serializable]
    public struct NeureShape : IShape
    {

        public NeureShape(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }

        public int Rows { get; set; }
        public int Columns { get; set; }
        public int Levels => Rows * Columns;
    }

    [Serializable]
    public struct DataShape : IShape
    {
        public DataShape(int rows, int columns, int layers = 1)
        {
            Rows = rows;
            Columns = columns;
            Layers = layers;
            Levels = Rows * Columns * Layers;
        }

        public int Rows { get; set; }
        public int Columns { get; set; }
        public int Layers { get; set; }
        public int Levels { get; set; }
    }
}
