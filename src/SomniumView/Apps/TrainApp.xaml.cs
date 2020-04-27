namespace SomniumView.Apps
{
    /// <summary>
    /// Interaction logic for TrainApp.xaml
    /// </summary>
    public partial class TrainApp
    {
        public TrainApp()
        {
            InitializeComponent();
            MTrainApp = new MTrainApp();
            DataContext = this;
        }

        public MTrainApp MTrainApp { set; get; }
    }
}
