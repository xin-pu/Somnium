using System;

namespace Somnium.Func
{
    public class Activate
    {
        public static double sigmoid(double u) => 1 / (1 + Math.Pow(Math.E, u));

        public static double tanh(double u) => (Math.Pow(Math.E, u) - Math.Pow(Math.E, -u)) / (Math.Pow(Math.E, u) + Math.Pow(Math.E, -u));

        public static double max(double u) => u >= 0 ? u : 0;

  
    }
}
