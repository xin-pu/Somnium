using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using Somnium.Kernel;

namespace Somnium.Core
{
    public class TrainDataManager : INotifyPropertyChanged,ICloneable
    {

        private LabelMap _labelMap;
        private List<StreamData> _streamDatas;
        private bool _formatStatus;
        private bool _isReading;
        private DataShape _dataShapeIn;
        private int _dataShapeOut;
        private IEnumerable<string> _labelsOut;
        private string _workFolder;
        private int _fileCount;


        public Func<string, StreamData> GetStreamData { set; get; }

        public LabelMap LabelMap
        {
            set => UpdateProperty(ref _labelMap, value);
            get => _labelMap;
        }

        public string WorkFolder
        {
            set => UpdateProperty(ref _workFolder, value);
            get => _workFolder;
        }

        public bool IsReading
        {
            set => UpdateProperty(ref _isReading, value);
            get => _isReading;
        }

        public DataShape DataShapeIn
        {
            set => UpdateProperty(ref _dataShapeIn, value);
            get => _dataShapeIn;
        }

        public int DataShapeOut
        {
            set => UpdateProperty(ref _dataShapeOut, value);
            get => _dataShapeOut;
        }

        public IEnumerable<string> LabelsOut
        {
            set => UpdateProperty(ref _labelsOut, value);
            get => _labelsOut;
        }

        public bool FormatStatus
        {
            set => UpdateProperty(ref _formatStatus, value);
            get => _formatStatus;
        }

        public int FileCount
        {
            set => UpdateProperty(ref _fileCount, value);
            get => _fileCount;
        }

        public List<StreamData> StreamDatas
        {
            set => UpdateProperty(ref _streamDatas, value);
            get => _streamDatas;
        }

        private TrainDataManager()
        {

        }

        public TrainDataManager(string workFolder, Func<string, StreamData> getStream)
        {
            WorkFolder = workFolder;
            GetStreamData = getStream;
            LoadStreamDatas();
            FilterDataShape(StreamDatas);
        }


        public void Binding(TrainParameters trainParameters)
        {
            StreamDatas.ForEach(a =>
            {
                a.GetCost = trainParameters.GetCost;
                a.GetLikelihood = trainParameters.GetLikelihood;
                a.GetEstimateLabel = LabelMap.GetLabel;
            });
        }


        private void LoadStreamDatas()
        {
            var dir = new DirectoryInfo(WorkFolder);
            if (!dir.Exists) return;
            IsReading = true;
            StreamDatas = dir.GetFiles()
                .Where(path => path.Extension == ".txt")
                .AsParallel()
                .Select(path =>
                {
                    var inputLayer = GetStreamData?.Invoke(path.FullName);
                    return inputLayer;
                }).ToList();
            IsReading = false;
        }

        private void FilterDataShape(List<StreamData> streamDatas)
        {
            var dataShapes = streamDatas.Select(a => a.InputDataShape).ToArray();
            if (dataShapes.Length > 1)
            {
                // if data shape in is not correct
            }

            LabelsOut = StreamDatas.Select(a => a.ExpectedLabel).Distinct().ToList();
            LabelMap = new LabelMap(LabelsOut);

            DataShapeIn = dataShapes.First();
            DataShapeOut = LabelsOut.Count();

            StreamDatas.ForEach(a => a.ExpectedOut = LabelMap.GetCorrectResult(a.ExpectedLabel));
            FileCount = StreamDatas.Count;

        }

        public object Clone()
        {
            return new TrainDataManager
            {
                StreamDatas = StreamDatas,
                LabelMap = LabelMap,
                DataShapeIn = DataShapeIn,
                DataShapeOut = DataShapeOut,
                LabelsOut = LabelsOut,
                WorkFolder = WorkFolder,
                FileCount = FileCount,
                GetStreamData = GetStreamData
            };
        }


        #region

        public void UpdateProperty<T>(ref T properValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (Equals(properValue, newValue))
            {
                return;
            }

            properValue = newValue;
            OnPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }


}
