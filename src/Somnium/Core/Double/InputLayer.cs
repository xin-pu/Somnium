namespace Somnium.Core.Double
{
    public class InputLayer : Layer<double>
    {

        public double ExpectVal { set; get; }

        
        public InputLayer(DataSize dataSize, double expectedVal) : base(dataSize)
        {
            ExpectVal = expectedVal;
        }

    }
}
