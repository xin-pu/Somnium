using System;
using System.Linq;

namespace Somnium.Func
{

    public class Activate
    {

        #region Sigmod Type

        /// <summary>
        /// Logistic Type Method
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static double Logistic(double u) => 1 / (1 + Math.Pow(Math.E, -u));


        public static double HardLogistic(double u) => new[] {new[] {0.25 * u + 0.5, 1}.Min(), 0}.Max();

        /// <summary>
        /// Logistic Type Methods
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static double Tanh(double u) => (Math.Pow(Math.E, u) - Math.Pow(Math.E, -u)) /
                                               (Math.Pow(Math.E, u) + Math.Pow(Math.E, -u));


        public static double HardTanh(double u) => new[] {new[] {u, 1}.Min(), -1}.Max();

        #endregion


        #region ReLU Tyle (Rectfied Linear Unit)

        public static double ReLU(double u) => u >= 0 ? u : 0;

        public static double LeakyReLU(double u, double y = 0.01) => u > 0 ? u : y * u;

        public static double ELU(double u, double y) => u > 0 ? u : y * (Math.Pow(Math.E, u) - 1);

        public static double Softplus(double u) => Math.Log(1 + Math.Pow(Math.E, u));

        #endregion


        #region Swish

        #endregion

        #region GELU

        #endregion

    }


    [Serializable]
    public enum ActivateType
    {
        Logistic,
        HardLogistic,
        Tanh,
        HardTanh,
        ReLU,
        LeakyReLU, 
        ELU,
        Softplus,
        Max
    }
}
