using System;
using System.Collections.Generic;
using System.Linq;

namespace Somnium.Func
{
    public class Cost
    {
        public static double GetVariance(IEnumerable<double> expectedVal, IEnumerable<double> estimatedVal)
        {
            var expected = expectedVal as double[] ?? expectedVal.ToArray();
            var estimated = estimatedVal as double[] ?? estimatedVal.ToArray();
            if (expected.Length != estimated.Length)
                return double.NaN;
            var variance = expected.Zip(estimated, (a, b) => Math.Pow(a - b, 2)).Sum() / 2;
            return variance;
        }
    }

    [Serializable]
    public enum CostType
    {
        Basic
    }
}
