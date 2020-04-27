using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using JetBrains.Annotations;
using Microsoft.WindowsAPICodePack.Dialogs;
using Somnium.Core;

namespace SomniumView.Apps
{
    public class MTrainApp : INotifyPropertyChanged
    {

        private string _workFolder;

        public MTrainApp()
        {
            LoadCommands();
        }


        public string WorkFolder
        {
            set => UpdateProperty(ref _workFolder, value);
            get => _workFolder;
        }
        public List<string> LoadDataMethod { set; get; }
        public string SelectedLoadDataMethod { set; get; }

        public TrainDataManager TrainDataManager { set; get; }

        public ICommand OpenWorkFolderCmd { set; get; }
        public ICommand LoadTrainDataSetsCmd { set; get; }




        private void LoadCommands()
        {
            OpenWorkFolderCmd = new RelayCommand(OpenWorkFolderExecute);
            LoadTrainDataSetsCmd = new RelayCommand(LoadTrainDataSets);
        }

        public void OpenWorkFolderExecute()
        {
            var folderBrowserDialog = new CommonOpenFileDialog {IsFolderPicker = true};
            var res = folderBrowserDialog.ShowDialog();
            if (res == CommonFileDialogResult.Ok) WorkFolder = folderBrowserDialog.FileName;
        }

        public void LoadTrainDataSets()
        {

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
