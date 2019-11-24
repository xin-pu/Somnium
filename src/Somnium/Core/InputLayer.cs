namespace Somnium.Core.Double
{

    /// <summary>
    /// Input Layer is the first Layer which the input size and output size are same.
    /// </summary>
    public class InputLayer : StandLayer
    {

        public double[] ExpectVal { set; get; }


        public void ConnectNextLayer(StandLayer nextLayer)
        {

        }

        public InputLayer(DataSize dataSize, double[] expectedVal) : base(dataSize)
        {
            ExpectVal = expectedVal;
        }

    }
}
