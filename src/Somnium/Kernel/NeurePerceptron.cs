using System;
using System.Linq;
using System.Xml.Serialization;
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

        [XmlIgnore] public Func<double, double> ActivateFuc { set; get; }
        [XmlIgnore] public Func<double, double> FirstDerivativeFunc { set; get; }


        private ActivateType _activateType;

        public ActivateType ActivateType
        {
            set
            {
                UpdateProperty(ref _activateType, value);
                switch (value)
                {
                    case ActivateType.Tanh:
                        ActivateFuc = Activate.Tanh;
                        break;
                    case ActivateType.Max:
                        ActivateFuc = Activate.Max;
                        break;
                    default:
                        ActivateFuc = Activate.Sigmoid;
                        break;
                }

                FirstDerivativeFunc = Differentiate.FirstDerivativeFunc(ActivateFuc);
            }
            get => _activateType;
        }



        public NeurePerceptron()
        {

        }

        public NeurePerceptron(NeureShape shape, ActivateType activateType = ActivateType.Sigmoid)
            : base(shape)
        {
            ActivateType = activateType;
        }

        public NeurePerceptron(int rows, int columns, ActivateType activateType = ActivateType.Sigmoid)
            : base(rows, columns)
        {
            ActivateType = activateType;
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
            lock (MyLock)
            {
                WeightDelta = (Matrix) (WeightDelta + devWeight);
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
