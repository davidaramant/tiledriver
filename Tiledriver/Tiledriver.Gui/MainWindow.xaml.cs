﻿using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Tiledriver.Core.FormatModels.Uwmf.Parsing;
using Tiledriver.Gui.ViewModels;

namespace Tiledriver.Gui
{
    public partial class MainWindow
    {
        private MainVm vm = new MainVm();

        private int tileSize = 24;

        private string packagedMapFilesDir = System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\MapFiles\\";

        public MainWindow()
        {
            InitializeComponent();

            DataContext = vm;
            vm.PropertyChanged += SubscribeMapCanvasToMapChanges;
            MapCanvas.NotifyNewMapItems += DetailPane.Update;
            KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.OemPlus || e.Key == Key.OemFinish)
            {
                ZoomIn(sender, e);
            }
            if (e.Key == Key.OemMinus)
            {
                ZoomOut(sender, e);
            }
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

        private void SelectDefaultMapFile(object sender, RoutedEventArgs e)
        {
            OpenMapFile(packagedMapFilesDir + "FIXEDTEXTMAP.txt");
        }

        private void SelectDemoMapFile(object sender, RoutedEventArgs e)
        {
            OpenMapFile(packagedMapFilesDir + "thingdemo.txt");
        }

        private void OpenMapFile(string filePath)
        {
            if (!File.Exists(filePath)) return;

            // TODO: Use some kind of dependency injection system
            using (var stream = File.OpenRead(filePath))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var sa = new UwmfSyntaxAnalyzer();
                vm.Map = UwmfParser.Parse(sa.Analyze(new UwmfLexer(textReader)));
                Application.Current.MainWindow.Title = $"Tiledriver UWMF Viewer - {vm.Map.Name}";
            }
        }

        private void QuitApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
