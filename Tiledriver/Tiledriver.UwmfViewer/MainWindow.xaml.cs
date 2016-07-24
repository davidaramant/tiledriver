using System.IO;
using Tiledriver.Core.Uwmf.Parsing;
using Tiledriver.UwmfViewer.ViewModels;

namespace Tiledriver.UwmfViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MainVm vm = new MainVm();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = vm;
            vm.PropertyChanged += SubscribeMapCanvasToMapChanges;
        }

        public void SubscribeMapCanvasToMapChanges(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(vm.Map)))
            {
                MapCanvas.Update(vm.Map);
            }
        }

        private void SelectMapFile(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog()
            {
                Multiselect = false
            };
            var result = dialog.ShowDialog();
            if (result == true && dialog.CheckFileExists)
            {
                OpenMapFile(dialog.FileName);
            }
        }

        private void OpenMapFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    vm.Map = Parser.Parse(new Lexer(new UwmfCharReader(stream)));
                    App.Current.MainWindow.Title = $"Tiledriver UWMF Viewer - {vm.Map.Name}";
                }
            }
        }

        private void QuitApplication(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
