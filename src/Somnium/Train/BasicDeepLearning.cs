using System;
using System.Linq;
using System.Threading.Tasks;
using Somnium.Kernel;

namespace Somnium.Train
{
    public class BasicDeepLearning:StandTrain
    {

        

        public override void ExecuteTrain(StreamLayer layerNet, TrainDataManager trainDataManager)
        {
            var inputStreams = trainDataManager.StreamDatas;
            StartTime = DateTime.Now;
            TrainCountCurrent = 0;

            for (TrainCountCurrent = 1; TrainCountCurrent <= TrainCountLimit; TrainCountCurrent++)
            {
                //以神经网络层更新数据层
                inputStreams.AsParallel().ForAll(singleStream => singleStream.ActivateLayerNet(layerNet));

                CorrectRateCurrent = inputStreams.Count(a => a.IsMeetExpect) * 100.0 / inputStreams.Count;
                CorrectRates.Add(CorrectRateCurrent);

                //反向传播误差
                inputStreams.AsParallel().ForAll(singleStream => singleStream.ErrorBackPropagation(layerNet));

                //从数据层收集误差并更新神经网络层的神经元
                layerNet.UpdateWeight(inputStreams);

            }

            StopTime = DateTime.Now;
        }


    }
}
