using System;
using System.Collections.Generic;
using System.Linq;
using Somnium.Core.Double;

namespace Somnium.Core
{
    public class ContactLayers
    {
        public static NerveCell<double> ContactLayerActivate(IEnumerable<Layer<double>> inputs,
            IEnumerable<LayerActivate> outputs)
        {
            var inputTemplat = inputs.First();
            var layerActivates = outputs as LayerActivate[] ?? outputs.ToArray();
            var nerveCell = new NerveCellActivate(inputTemplat.RowCount, inputTemplat.ColumnCount)
            {
                OutputLevel = layerActivates.Count()
            };
            layerActivates.ToList().ForEach(output => output.NerveCell = nerveCell);
            return nerveCell;
        }

    }
}
