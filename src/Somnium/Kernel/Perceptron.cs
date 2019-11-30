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
                ActivateFuc = Activate.Sigmoid;
                FirstDerivativeFunc = Differentiate.FirstDerivativeFunc(ActivateFuc);
            }
            get { return _activateMode; }
        }

        [XmlIgnore]
        public Func<double, double> ActivateFuc { set; get; }
        [XmlIgnore]
        public Func<double, double> FirstDerivativeFunc { set; get; }


        public Perceptron()
        {

        }

        public Perceptron(NeureDegree degree):base(degree)
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
