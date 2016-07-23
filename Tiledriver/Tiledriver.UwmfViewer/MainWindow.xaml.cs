namespace Tiledriver.UwmfViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenMapFile(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog() { Multiselect = false };
            var result = dialog.ShowDialog();
            if (result == true)
            {
                App.Current.MainWindow.Title = $"Tiledriver UWMF Viewer - {dialog.SafeFileName}";
            }
        }

        private void QuitApplication(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
