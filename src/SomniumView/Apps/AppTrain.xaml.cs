namespace SomniumView.Apps
{
    /// <summary>
    /// Interaction logic for AppTrain.xaml
    /// </summary>
    public partial class AppTrain
    {
        public AppTrain()
        {
            InitializeComponent();
            AppTrainModel = new AppTrainModel();
            DataContext = this;
        }

        public AppTrainModel AppTrainModel { set; get; }
    }
}
