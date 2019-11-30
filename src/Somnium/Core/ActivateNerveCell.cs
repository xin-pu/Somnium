using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Func;

namespace Somnium.Core
{
 
    [Serializable]
    public class ActivateNerveCell 
    {
        private readonly object _myLock = new object();
        private ActivateMode _activateModeMode;
        private List<double> _weightArray=new List<double>();
        private List<double> _deltaWeightArray=new List<double>();

        [XmlIgnore]
        public Func<double, double> ActivateFuc { set; get; }
        [XmlIgnore]
        public Func<double, double> FirstDerivativeFunc { set; get; }
        [XmlIgnore]
        public Matrix<double> Weight { set; get; }
        [XmlIgnore]
        public Matrix<double> DeltaWeight { set; get; }
        public List<double> WeightArray
        {
            set
            {
                _weightArray = value;
                Weight=new DenseMatrix(DataSize.RowCount,DataSize.ColumnCount,value.ToArray());
            }
            get => _weightArray;
        }

        public List<double> DeltaWeightArray
        {
            set
            {
                _deltaWeightArray = value;
                DeltaWeight = new DenseMatrix(DataSize.RowCount, DataSize.ColumnCount, value.ToArray());
            }
            get => _deltaWeightArray;
        }

        public double Bias { set; get; }
        public double DeltaBias { set; get; }

        public DataSize DataSize { set; get; }
        public DataSize OutputLevel { set; get; }


        public ActivateNerveCell()
        {

        }

        public ActivateNerveCell(DataSize dataSize)
        {
            DataSize = dataSize;
            Weight = DenseMatrix.CreateRandom(dataSize.RowCount, dataSize.ColumnCount, new ContinuousUniform());
            WeightArray = ((DenseMatrix)Weight).Values.ToList();
            Bias = new ContinuousUniform().Median;
            DeltaWeight = DenseMatrix.CreateDiagonal(dataSize.RowCount, dataSize.ColumnCount, 0);
            DeltaWeightArray = ((DenseMatrix)DeltaWeight).Values.ToList();
            DeltaBias = 0;
            ActivateModeMode = Func.ActivateMode.Sigmoid;
        }


        public double WeightedInput { set; get; }
        public double ActivateOutput { set; get; }
        public double Deviation { set; get; }
        public ActivateMode ActivateModeMode
        {
            set
            {
                _activateModeMode = value;
                ActivateFuc = Activate.Sigmoid;
                FirstDerivativeFunc = Differentiate.FirstDerivativeFunc(ActivateFuc);
            }
            get { return _activateModeMode; }
        }



        public void Activated(Matrix inputData)
        {
            WeightedInput = inputData.PointwiseMultiply(Weight).Enumerate().Sum();
            ActivateOutput = ActivateFuc(inputData.PointwiseMultiply(Weight).Enumerate().Sum() + Bias);
        }

        public double GetActivated(Matrix inputData)
        {
            var weightedInput = inputData.PointwiseMultiply(Weight).Enumerate().Sum();
            var activateOutput = ActivateFuc(weightedInput + Bias);
            return activateOutput;
        }

        public void Updated()
        {
            Weight = (Matrix) (Weight + DeltaWeight);
            Bias += DeltaBias;
            DeltaWeight.Clear();
            DeltaBias = 0;
        }

        public void AddDeviation(Matrix devWeight, double devBias)
        {
            lock (_myLock)
            {
                DeltaWeight = (Matrix)(DeltaWeight + devWeight);
                DeltaBias = DeltaBias + devBias;
            }
        }
    }
}
