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

        private ActivateMode _activateMode = ActivateMode.Sigmoid;

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

        public NeurePerceptron(NeureShape shape, ActivateMode activateMode = ActivateMode.Sigmoid) :base(shape)
        {
            ActivateMode = activateMode;
        }

        public NeurePerceptron(int rows, int columns, ActivateMode activateMode = ActivateMode.Sigmoid)
            : base(rows, columns)
        {
            ActivateMode = activateMode;
        }

        /// <summary>
        /// Activate Neure
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public override Tuple<double, double> Activated(Matrix inputData)
        {
            var weight = inputData.PointwiseMultiply(Weight).Enumerate().Sum();
            var activate = ActivateFuc(weight + Offset);
            return new Tuple<double, double>(activate, weight);
        }


        public override void AddDeviation(Matrix devWeight, double devBias)
        {
            lock (_myLock)
            {
                WeightDelta = (Matrix)(WeightDelta + devWeight);
                OffsetDelta = OffsetDelta + devBias;
            }
        }

        public override void UpdateDeviation()
        {
            Weight = (Matrix) (Weight + WeightDelta);
            Offset += OffsetDelta;
            WeightDelta.Clear();
            OffsetDelta = 0;
        }
    }
}
