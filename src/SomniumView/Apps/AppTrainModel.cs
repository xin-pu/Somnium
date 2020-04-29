using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using JetBrains.Annotations;
using Microsoft.WindowsAPICodePack.Dialogs;
using Somnium.Core;
using Somnium.Data;
using Somnium.Learner;
using Somnium.Utility;

namespace SomniumView.Apps
{
    public class AppTrainModel : INotifyPropertyChanged
    {


        #region ICommands

        public ICommand OpenWorkFolderCmd { set; get; }
        public ICommand LoadTrainDataSetsCmd { set; get; }
        public ICommand AddLayerCmd { set; get; }
        public ICommand ClearLayerCmd { set; get; }
        public ICommand DeleteLayerCmd { set; get; }
        public ICommand CreateLearnerCmd { set; get; }

        #endregion


        private string _workFolder;
        private TrainDataManager _trainDataManager;
        private TrainParameters _trainParameters;
        private AsyncObservableCollection<DeepLearner> _deepLearners;

        private List<Type> _dataReaders;

        public AppTrainModel()
        {
            LoadCommands();
            LoadInitialInfo();
        }


        public string WorkFolder
        {
            set => UpdateProperty(ref _workFolder, value);
            get => _workFolder;
        }

        public List<Type> DataReaders
        {
            set
            {
                UpdateProperty(ref _dataReaders, value);
                SelectedDataReader = value.First();
            }
            get => _dataReaders;
        }

        public Type SelectedDataReader { set; get; }


        public TrainDataManager TrainDataManager
        {
            set => UpdateProperty(ref _trainDataManager, value);
            get => _trainDataManager;
        }

        public TrainParameters TrainParameters
        {
            set => UpdateProperty(ref _trainParameters, value);
            get => _trainParameters;
        }

        public AsyncObservableCollection<DeepLearner> DeepLearners
        {
            set => UpdateProperty(ref _deepLearners, value);
            get => _deepLearners;
        }


        private void LoadInitialInfo()
        {
            DataReaders = DataReader.GetDataReaders();
            TrainParameters = new TrainParameters();
            DeepLearners = new AsyncObservableCollection<DeepLearner>();
        }

        private void LoadCommands()
        {
            OpenWorkFolderCmd = new RelayCommand(OpenWorkFolderExecute);
            LoadTrainDataSetsCmd = new RelayCommand(LoadTrainDataSetsExecute);
            AddLayerCmd = new RelayCommand(AppendDefaultLayerExecute);
            ClearLayerCmd = new RelayCommand(ClearLayerExecute);
            CreateLearnerCmd=new RelayCommand(CreateLeanerExecute);
        }


        public void OpenWorkFolderExecute()
        {
            var folderBrowserDialog = new CommonOpenFileDialog {IsFolderPicker = true};
            var res = folderBrowserDialog.ShowDialog();
            if (res == CommonFileDialogResult.Ok) WorkFolder = folderBrowserDialog.FileName;
        }

        public void LoadTrainDataSetsExecute()
        {
            TrainDataManager = new TrainDataManager(WorkFolder,
                new ResizeDigitsDataReader().ReadStreamData);
        }

        public void AppendDefaultLayerExecute()
        {
            TrainParameters.AppendDefaultLayer();
        }

        public void ClearLayerExecute()
        {
            TrainParameters.ClearLayer();
        }

        public void CreateLeanerExecute()
        {
            if (TrainDataManager == null) return;
            // Clone a new trainParameters.
            var trainParameters = (TrainParameters) TrainParameters.Clone();
            var trainDataManager = (TrainDataManager) TrainDataManager.Clone();
            trainDataManager.Binding(trainParameters);

            var deepLearner = new DeepLearner(trainDataManager, trainParameters);


            DeepLearners.Add(deepLearner);
            //deepLearner.ExecuteTrain(layNet, TrainDataManager);
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
