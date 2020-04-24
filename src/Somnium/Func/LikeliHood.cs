using System;
using System.Collections.Generic;
using System.Linq;

namespace Somnium.Func
{
    public class LikeliHood
    {
        public static IEnumerable<double> SoftMax(IEnumerable<double> activated)
        {
            var exps = activated.Select(a => Math.Pow(Math.E, a)).ToArray();
            var expsSum = exps.Sum();
            return exps.Select(a => a / expsSum).ToList();
        }
    }

    public enum LikeliHoodType
    {
        SoftMax
    }
}
