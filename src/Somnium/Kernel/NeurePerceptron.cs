using System;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Func;

namespace Somnium.Kernel
{
    /// <summary>
    /// 感知机神经元
    /// </summary>
    [Serializable]
    public class NeurePerceptron : Neure
    {

        private ActivateMode _activateMode;

        public ActivateMode ActivateMode
        {
            set
            {
                _activateMode = value;
                switch (value)
                {
                    case ActivateMode.Tanh:
                        ActivateFuc = Activate.Tanh;
                        break;
                    case ActivateMode.Max:
                        ActivateFuc = Activate.Max;
                        break;
                    default:
                        ActivateFuc = Activate.Sigmoid;
                        break;
                }
              
                FirstDerivativeFunc = Differentiate.FirstDerivativeFunc(ActivateFuc);
            }
            get => _activateMode;
        }

    
        public Func<double, double> ActivateFuc { set; get; }
        public Func<double, double> FirstDerivativeFunc { set; get; }
        
        public NeurePerceptron()
        {
              
        }

        public NeurePerceptron(NeureShape shape):base(shape)
        {
          
        }

        public NeurePerceptron(int rows, int columns) : base(rows, columns)
        {

        }

        /// <summary>
        /// Activate Neure
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public Tuple<double, double> GetActivated(double[] inputData)
        {
            if (inputData.Length != Shape.Levels)
                return new Tuple<double, double>(double.NaN, double.NaN);
            var weight = inputData.Zip(Weight, (a, b) => a * b).Sum();
            var activate = ActivateFuc(weight + Offset);
            return new Tuple<double, double>(activate, weight);
        }

        public Tuple<double, double> GetActivated(Matrix inputData)
        {
            var values = inputData.AsRowMajorArray();
            return GetActivated(values);
        }
    }
}
