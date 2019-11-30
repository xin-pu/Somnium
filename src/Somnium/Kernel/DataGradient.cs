using System.Collections.Generic;

namespace Somnium.Kernel
{
    public class DataGradient
    {
        public readonly  object Locker=new object();
        public Dictionary<int,Perceptron[]> Gradients { set; get; }

        public void AddGradients(LayerFullConnected layer)
        {
            Gradients[layer.LayerIndex] = (Perceptron[])layer.Perceptrons.Clone();
        }

        public void AddGradients(LayerOutput layer)
        {
            
        }
    }
}
