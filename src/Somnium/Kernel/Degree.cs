using System;

namespace Somnium.Kernel
{
    [Serializable]
    public struct NeureShape
    {
        public int Rows;
        public int Columns;
        public int Levels => Rows * Columns;
    }

    [Serializable]
    public struct DataShape
    {
        public int Rows;
        public int Columns;
        public int Layers;
        public int Levels => Rows * Columns * Layers;
    }
}
