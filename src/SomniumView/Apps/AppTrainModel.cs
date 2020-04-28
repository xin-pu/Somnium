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

namespace SomniumView.Apps
{
    public class AppTrainModel : INotifyPropertyChanged
    {

        private string _workFolder;
        private TrainDataManager _trainDataManager;
        private TrainParameters _trainParameters;
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

        public ICommand OpenWorkFolderCmd { set; get; }
        public ICommand LoadTrainDataSetsCmd { set; get; }

        public ICommand AddLayerCmd { set; get; }
        public ICommand ClearLayerCmd { set; get; }
        public ICommand DeleteLayerCmd { set; get; }


        private void LoadInitialInfo()
        {
            DataReaders = DataReader.GetDataReaders();
            TrainParameters = new TrainParameters();
        }

        private void LoadCommands()
        {
            OpenWorkFolderCmd = new RelayCommand(OpenWorkFolderExecute);
            LoadTrainDataSetsCmd = new RelayCommand(LoadTrainDataSets);
            AddLayerCmd = new RelayCommand(AppendDefaultLayer);
            ClearLayerCmd = new RelayCommand(ClearLayer);
        }


        public void OpenWorkFolderExecute()
        {
            var folderBrowserDialog = new CommonOpenFileDialog {IsFolderPicker = true};
            var res = folderBrowserDialog.ShowDialog();
            if (res == CommonFileDialogResult.Ok) WorkFolder = folderBrowserDialog.FileName;
        }

        public void LoadTrainDataSets()
        {
            TrainDataManager = new TrainDataManager(WorkFolder, new ResizeDigitsDataReader().ReadStreamData);
        }

        public void AppendDefaultLayer()
        {
            TrainParameters.AppendDefaultLayer();
        }

        public void ClearLayer()
        {
            TrainParameters.ClearLayer();
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
