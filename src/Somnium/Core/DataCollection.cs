using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;

namespace Somnium.Core
{
    /// <summary>
    /// datacollectin is containner for all data by one input
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataCollection<T>
        where T : struct, IEquatable<T>, IFormattable
    {


        public IList<Matrix<T>> NerveCellDatas { set; get; }
    }
}
