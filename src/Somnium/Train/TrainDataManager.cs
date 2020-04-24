using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Data;
using Somnium.Kernel;

namespace Somnium.Train
{
    public class TrainDataManager: INotifyPropertyChanged
    {
        private string _workFolder;
        private List<StreamData> _streamDatas;
        private DataShape _dataShapeFormat;

        private LabelMap LabelMap { set; get; }

        public string WorkFolder
        {
            set
            {
                UpdateProperty(ref _workFolder, value);
                LoadStreamDatas();
            }
            get => _workFolder;
        }

        public DataShape DataShapeFormat
        {
            set => UpdateProperty(ref _dataShapeFormat, value);
            get => _dataShapeFormat;
        }

        public List<StreamData> StreamDatas
        {
            set => UpdateProperty(ref _streamDatas, value);
            get => _streamDatas;
        }

        public int OutTypeCount => LabelMap.Count;


        public TrainDataManager(string workFolder)
        {
            WorkFolder = workFolder;
        }

        public void LoadStreamDatas()
        {
            StreamData.GetStreamData = GetArrayStreamData;

            var dir = new DirectoryInfo(WorkFolder);
            if (!dir.Exists) return;
            StreamDatas = dir.GetFiles()
                .Where(path => path.Extension == ".txt")
                .Select(path =>
                {
                    var inputLayer = StreamData.CreateStreamData(path.FullName);
                    return inputLayer;
                }).ToList();
            DataShapeFormat = StreamData.FilterDataShape(StreamDatas);
            LabelMap = new LabelMap(StreamDatas.Select(a => a.ExpectedLabel));
            StreamDatas.ForEach(a => a.ExpectedOut = LabelMap.GetCorrectResult(a.ExpectedLabel));
            StreamData.GetEstimateLabel = LabelMap.GetLabel;
        }

        public static StreamData GetArrayStreamData(string path)
        {
            using var streamRead = new StreamReader(path);
            var allData = new List<double>();
            var allLine = streamRead.ReadToEnd();
            var lines = allLine.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            lines.ForEach(line => { allData.AddRange(line.ToCharArray().Select(a => double.Parse(a.ToString()))); });

            var matrix = new DenseMatrix(allData.Count, 1);
            matrix.SetColumn(0, allData.ToArray());
            var actual = new FileInfo(path).Name.Split('_').First();
            return new StreamData
            {
                InputDataMatrix = matrix,
                ExpectedLabel = actual,
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
