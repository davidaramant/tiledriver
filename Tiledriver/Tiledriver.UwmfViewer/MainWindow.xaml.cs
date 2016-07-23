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

        private void OpenMapFile(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog()
            {
                Multiselect = false,
                FileName = "TEXTMAP.txt"
            };
            var result = dialog.ShowDialog();
            if (result == true && dialog.CheckFileExists)
            {
                App.Current.MainWindow.Title = $"Tiledriver UWMF Viewer - {dialog.SafeFileName}";
                using (var stream = dialog.OpenFile())
                {
                    vm.Map = Parser.Parse(new Lexer(new UwmfCharReader(stream)));
                }
            }
        }

        private void QuitApplication(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
