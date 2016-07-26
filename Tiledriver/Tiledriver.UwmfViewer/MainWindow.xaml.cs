using System;
using System.IO;
using System.Windows;
using Tiledriver.Core.Uwmf.Parsing;
using Tiledriver.UwmfViewer.ViewModels;

namespace Tiledriver.UwmfViewer
{
    public partial class MainWindow
    {
        private MainVm vm = new MainVm();

        private int tileSize = 24;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = vm;
            vm.PropertyChanged += SubscribeMapCanvasToMapChanges;
            MapCanvas.NotifyNewMapItems += DetailPane.Update;
        }

        private void SubscribeMapCanvasToMapChanges(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(vm.Map)))
            {
                MapCanvas.Update(vm.Map, tileSize);
                ZoomInButton.IsEnabled = true;
                ZoomOutButton.IsEnabled = true;
            }
        }

        private void ZoomIn(object sender, RoutedEventArgs e)
        {
            ZoomOutButton.IsEnabled = true;
            tileSize = Math.Min(tileSize + 8, 32);
            if (tileSize == 32) ZoomInButton.IsEnabled = false;
            MapCanvas.Update(vm.Map, tileSize);
        }

        private void ZoomOut(object sender, RoutedEventArgs e)
        {
            ZoomInButton.IsEnabled = true;
            tileSize = Math.Max(tileSize - 8, 8);
            if (tileSize == 8) ZoomOutButton.IsEnabled = false;
            MapCanvas.Update(vm.Map, tileSize);
        }

        private void SelectMapFile(object sender, RoutedEventArgs e)
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
            if (!File.Exists(fileName)) return;

            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                vm.Map = Parser.Parse(new Lexer(new UwmfCharReader(stream)));
                Application.Current.MainWindow.Title = $"Tiledriver UWMF Viewer - {vm.Map.Name}";
            }
        }

        private void QuitApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
