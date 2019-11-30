using System;

namespace Somnium.Func
{
    public class Activate
    {
        public static double Sigmoid(double u) => 1 / (1 + Math.Pow(Math.E, u));

        public static double Tanh(double u) => (Math.Pow(Math.E, u) - Math.Pow(Math.E, -u)) /
                                               (Math.Pow(Math.E, u) + Math.Pow(Math.E, -u));
        public static double Max(double u) => u >= 0 ? u : 0;

    }

    [Serializable]
    public enum ActivateMode
    {
        Sigmoid,
        Tanh,
        Max
    }
}
