using System.Threading.Tasks;
using Somnium.Kernel;

namespace Somnium.Train
{
    public abstract class Train
    {
        public string Name { set; get; }
        public double Gradient { set; get; }
        public uint TrainCount { set; get; }
        public DataShape DataShapeIn { set; get; }
        public StreamLayer StreamLayer { set; get; }


        public abstract Task ExecuteTrain();

    }

}
