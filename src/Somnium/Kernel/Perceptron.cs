using System;
using System.Linq;
using System.Xml.Serialization;
using MathNet.Numerics;
using Somnium.Func;

namespace Somnium.Kernel
{
    public class Perceptron : Neure
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

        [XmlIgnore]
        public Func<double, double> ActivateFuc { set; get; }
        [XmlIgnore]
        public Func<double, double> FirstDerivativeFunc { set; get; }


        public Perceptron()
        {

        }

        public Perceptron(NeureShape shape):base(shape)
        {
          
        }

        public Perceptron(int rows, int columns) : base(rows, columns)
        {

        }

        public Tuple<double, double> GetActivated(double[] inputData)
        {
            var weight = inputData.Zip(Weight, (a, b) => a * b).Sum();
            var activate = ActivateFuc(weight + Offset);
            return new Tuple<double, double>(activate, weight);
        }

    }
}
