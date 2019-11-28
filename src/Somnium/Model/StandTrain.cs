using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Somnium.Core;
using Somnium.Data;

namespace Somnium.Model
{
    [Serializable]
    public abstract class StandTrain
    {
        public StandTrain()
        {
        }

        protected StandTrain(int iterations, double gradient)
        {
            Iterations = iterations;
            Gradient = gradient;
        }


        public Func<string, InputLayer> ReadLayerInput;
        public int Iterations { set; get; }
        public double Gradient { set; get; }
        public LabelMap LabelMap { set; get; }


        public abstract void CreateTrainModel(string trainFile);

        public abstract void Training(string trainFolder);

        public abstract void TestStandTrain(string testPath);

        public virtual StandTrain LoadStandTrain(string path)
        {
            using var fs = new FileStream(path, FileMode.Open);
            var formatter = new XmlSerializer(typeof(StandTrain));
            return (StandTrain) formatter.Deserialize(fs);
        }





    }


}
