using System;

namespace Somnium.Kernel
{
    [Serializable]
    public struct NeureDegree
    {
        public int Rows;
        public int Columns;
        public int Levels => Rows * Columns;
    }

    [Serializable]
    public struct DataDegree
    {
        public int Rows;
        public int Columns;
        public int Layers;
        public int Levels => Rows * Columns * Layers;
    }
}
