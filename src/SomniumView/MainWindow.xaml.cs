using System;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;

namespace SomniumView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            InitialAction();
        }

        private readonly object _lockForDialog = new object();

        public static Action<string> ShowMessageBoxAction;
        public static Action<string, string> ShowMessageBoxWithTitleAction;
        public static Func<string, string, bool> ShowYesOrNoMessageBoxFunc;
        public static Func<string, string, string> ShowInputBoxFunc;
        public static Func<string, string, Task<ProgressDialogController>> ShowProgressBoxFunc;

        private void InitialAction()
        {
            ShowMessageBoxAction = ShowMessageBox;
            ShowMessageBoxWithTitleAction = ShowMessageBox;
            ShowYesOrNoMessageBoxFunc = ShowYesOrNoMessageBox;
            ShowInputBoxFunc = ShowInputBox;
            ShowProgressBoxFunc = ShowProgressBox;
        }

        private void ShowMessageBox(string msg, string title)
        {
            lock (_lockForDialog)
            {
                Dispatcher?.Invoke(
                    () => { this.ShowModalMessageExternal(title, msg); });
            }
        }

        private void ShowMessageBox(string msg)
        {
            lock (_lockForDialog)
            {
                Dispatcher?.Invoke(
                    () => { this.ShowModalMessageExternal("Information", msg); });
            }
        }

        private bool ShowYesOrNoMessageBox(string msg, string title)
        {
            lock (_lockForDialog)
            {
                var res = Dispatcher?.Invoke(
                    () => this.ShowModalMessageExternal(title, msg, MessageDialogStyle.AffirmativeAndNegative));
                return res.Equals(MessageDialogResult.Affirmative);
            }
        }

        public string ShowInputBox(string msg, string title)
        {
            lock (_lockForDialog)
            {
                var res = Dispatcher?.Invoke(
                    () => this.ShowModalInputExternal(title, msg));
                return res;
            }
        }


        public async Task<ProgressDialogController> ShowProgressBox(string title, string msg)
        {
            var control = await Dispatcher?.InvokeAsync(
                async () => await this.ShowProgressAsync(title, msg));
            await Task.Delay(500);
            return control.Result;
        }

    }
}
