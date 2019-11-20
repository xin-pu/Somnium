using MathNet.Numerics.LinearAlgebra;
using System;

namespace Somnium.Core
{
    public abstract class Attach<T>:ICloneable
        where T:struct
    {
        

        public Matrix<double> Weight;
        public double Bias{ set; get; }

        public Func<T, T> ActivateFuc { set; get; }

        public int RowCount => Weight.RowCount;
        public int Column => Weight.ColumnCount;

        public object Clone()
        {
            throw new NotImplementedException();
        }



    }
}
