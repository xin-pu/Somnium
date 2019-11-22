using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Somnium.Core.Double;
using Somnium.Datas;

namespace Somnium.Tests
{
    [TestClass]
    public class ModelTest
    {
        [TestMethod]
        public void MatrixTest()
        {
            
        }

        [TestMethod]
        public void LoadMultiInputLay()
        {
            var dir = new DirectoryInfo(@"E:\Document Code\Code Pensonal\Somnium\datas\trainingDigits");
            var inputsLay = dir.GetFiles()
                .Select((path, index) =>
                {
                    var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadLayerInput);
                    inputLayer.LayerRowIndex = index;
                    inputLayer.LayerColumnIndex = 1;
                    return inputLayer;
                }).ToList();
            
        }

        [TestMethod]
        public void LoadInputLay()
        {
//            var res = ImageLoad.ReadLayerInput(@"E:\Document Code\Code Pensonal\Somnium\datas\trainingDigits\0_0.txt");
//            var ActivateCellsLay1 = Enumerable.Range(0, 20).ToList()
//                .Select(i => new ActivateNerveCell(res.DataSize))
//                .ToList();
//            var Lay2 = new ActivateNerveLayer(2, ActivateCellsLay1.ToList());
//            Lay2.Activated(res.DatasOutput);
//
//            var ActivateCellsLay2 = Enumerable.Range(0, 10).ToList()
//                .Select(i => new ActivateNerveCell(Lay2.DataSize))
//                .ToList();
//            var Lay3 = new OutputLayer(3, ActivateCellsLay2);
//            Lay3.Activated(Lay2.DatasOutput);

        }

        [TestMethod]
        public void LoadInputLays()
        {
//            var res = ImageLoad.ReadLayerInputs(@"E:\Document Code\Code Pensonal\Somnium\datas\testDigits");
//            var expected = res.Select(a => a.ExpectVal).Distinct();
//
//            var ActivateCellsLay1 = Enumerable.Range(0, 20).ToList()
//                .Select(i => new ActivateNerveCell(res.First().DataSize))
//                .ToList();
//
//            var Lay2 = new ActivateNerveLayer(2, ActivateCellsLay1.ToList());
//            Lay2.Activated(res.First().DatasOutput);
//
//            var ActivateCellsLay2 = Enumerable.Range(0, 10).ToList()
//                .Select(i => new ActivateNerveCell(Lay2.DataSize))
//                .ToList();
//            var Lay3 = new OutputLayer(3, ActivateCellsLay2);
//            Lay3.Activated(Lay2.DatasOutput);
//
//            var Lay2s = res.Select(a =>
//            {
//                var lay = new ActivateNerveLayer(2, ActivateCellsLay1.ToList());
//                lay.Activated(a.DatasOutput);
//                return lay;
//            });
//
//            var Lay3s = Lay2s.Select(a =>
//            {
//                var lay = new OutputLayer(3, ActivateCellsLay2.ToList());
//                lay.Activated(a.DatasOutput);
//                return lay;
//            });

        }


    }
}
