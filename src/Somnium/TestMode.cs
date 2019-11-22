using System.Collections.Generic;
using Somnium.Core;
using Somnium.Core.Double;

namespace Somnium
{
    /// <summary>
    /// Basic Model with Input, OutPut and One Activate Layer
    /// </summary>
    public class TestMode
    {
        public BasicModeConfig Config { set; get; }

        public TestMode(BasicModeConfig config)
        {
            InputLayer=new InputLayer();

        }

        public InputLayer InputLayer { set; get; }

        public ActivateNerveLayer ActivateNerveLayer { set; get; }

        public OutputLayer OutputLayer { set; get; }

        public IEnumerable<double> RightData { set; get; }


    }

    public struct BasicModeConfig
    {
        public DataSize InPutSize { set; get; }
        public int ActivateSize { set; get; }
        public int OutputSize { set; get; }
        
    }

}
