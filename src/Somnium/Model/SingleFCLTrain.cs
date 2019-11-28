using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Somnium.Core;
using Somnium.Data;

namespace Somnium.Model
{
    public class SingleFclTrain : StandTrain
    {

        public SingleFclTrain()
        {
        }

        public SingleFclTrain(int iterations,int fclCellCount, double gradient) : base(iterations, gradient)
        {
            FclCellCount = fclCellCount;
        }

        public int FclCellCount { set; get; }
        public int OutputTypeCount { set; get; }
        

        public InputLayer InputLayer { set; get; }

        public FullConnectedLayer FullConnectedLayer { set; get; }

        public OutputLayer OutputLayer { set; get; }

        public override void CreateTrainModel(string trainFolder)
        {
            var inputsLays = new DirectoryInfo(trainFolder).GetFiles()
                .Select((path, index) =>
                {
                    var inputLayer = ImageLoad.ReadLayerInput(path.FullName, ImageLoad.ReadDigitsAsInputLayer);
                    return inputLayer;
                }).ToList();

            LabelMap = new LabelMap(inputsLays.Select(a => a.Label));
            OutputTypeCount = LabelMap.Count;

            inputsLays.ForEach(a => a.ExpectVal = LabelMap.GetCorrectResult(a.Label));
            InputLayer = inputsLays.First();
            FullConnectedLayer = new FullConnectedLayer(InputLayer.OutputDataSizeFormat, FclCellCount);
            OutputLayer = new OutputLayer(FullConnectedLayer.OutputDataSizeFormat, OutputTypeCount);
        }

        public override void Training(string trainFolder)
        {
            if (ReadLayerInput == null) return;
        }

        public override void TestStandTrain(string testPath)
        {
            if (ReadLayerInput == null) return;
            throw new NotImplementedException();
        }

        public virtual void SaveTrain(string path)
        {
            using var fs = new FileStream(path, FileMode.Create);
            var formatter = new XmlSerializer(typeof(SingleFclTrain));
            formatter.Serialize(fs, this);
        }
    }
}
