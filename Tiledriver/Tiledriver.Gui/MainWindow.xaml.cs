// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Uwmf.Parsing;
using Tiledriver.Core.FormatModels.Wad;
using Tiledriver.Gui.ViewModels;

namespace Tiledriver.Gui
{
    public partial class MainWindow
    {
        private static string ECWolfPathConfigurationFile = "ECWolfPath.txt";

        private readonly MainVm _vm = new MainVm();

        private int _tileSize = 24;

        private readonly string _packagedMapFilesDir = System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\MapFiles\\";

        public MainWindow()
        {
            InitializeComponent();

            DataContext = _vm;
            _vm.PropertyChanged += SubscribeMapCanvasToMapChanges;
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
            if (e.PropertyName.Equals(nameof(_vm.MapData)))
            {
                MapCanvas.Update(_vm.MapData, _tileSize);
                ZoomInButton.IsEnabled = true;
                ZoomOutButton.IsEnabled = true;
            }
        }

        private void ZoomIn(object sender, RoutedEventArgs e)
        {
            ZoomOutButton.IsEnabled = true;
            _tileSize = Math.Min(_tileSize + 8, 32);
            if (_tileSize == 32) ZoomInButton.IsEnabled = false;
            MapCanvas.Update(_vm.MapData, _tileSize);
        }

        private void ZoomOut(object sender, RoutedEventArgs e)
        {
            ZoomInButton.IsEnabled = true;
            _tileSize = Math.Max(_tileSize - 8, 8);
            if (_tileSize == 8) ZoomOutButton.IsEnabled = false;
            MapCanvas.Update(_vm.MapData, _tileSize);
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
            OpenUwmfTextFile(_packagedMapFilesDir + "FIXEDTEXTMAP.txt");
        }

        private void SelectDemoMapFile(object sender, RoutedEventArgs e)
        {
            OpenUwmfTextFile(_packagedMapFilesDir + "thingdemo.txt");
        }

        private void OpenUwmfTextFile(string filePath)
        {
            if (!File.Exists(filePath)) return;

            // TODO: Use some kind of dependency injection system
            using (var stream = File.OpenRead(filePath))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var sa = new UwmfSyntaxAnalyzer();
                SetMap(UwmfParser.Parse(sa.Analyze(new UwmfLexer(textReader))));
            }
        }

        private void OpenMapFile(string filePath)
        {
            switch (Path.GetExtension(filePath)?.ToLowerInvariant())
            {
                case ".txt":
                case ".uwmf":
                    OpenUwmfTextFile(filePath);
                    break;

                case ".wad":
                    OpenWadFile(filePath);
                    break;
            }
        }

        private void OpenWadFile(string filePath)
        {
            var wad = WadFile.Read(filePath);

            var mapBytes = wad[1].GetData();
            using (var ms = new MemoryStream(mapBytes))
            using (var textReader = new StreamReader(ms, Encoding.ASCII))
            {
                var sa = new UwmfSyntaxAnalyzer();
                var map = UwmfParser.Parse(sa.Analyze(new UwmfLexer(textReader)));

                SetMap(map);
            }
        }

        private void RunCurrentMap(object sender, RoutedEventArgs e)
        {
            if (_vm.MapData == null)
            {
                MessageBox.Show(
                    "This will run the current map in ecwolf.exe. " +
                    "Open a map and try again.",
                    "No Map Loaded",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }
            LoadMapInEcWolf(_vm.MapData, "temp.wad");
        }

        private void SetMap(MapData map)
        {
            _vm.MapData = map;
            Application.Current.MainWindow.Title = $"Tiledriver - {_vm.MapData.Name}";
        }

        private void QuitApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private static void LoadMapInEcWolf(MapData uwmfMap, string wadPath)
        {
            if (!File.Exists(ECWolfPathConfigurationFile))
            {
                MessageBox.Show(
                    $"Could not find {ECWolfPathConfigurationFile}. " +
                    "Create this file in the output directory containing a single line with the full path to ECWolf.exe. " +
                    "Do not quote the path.",
                    "Missing configuration file",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            var ecWolfPath = File.ReadAllLines(ECWolfPathConfigurationFile).Single();

            if (Path.GetFileName(ecWolfPath).ToLowerInvariant() != "ecwolf.exe")
            {
                ecWolfPath = Path.Combine(ecWolfPath, "ecwolf.exe");
            }

            if (!File.Exists(ecWolfPath))
            {
                MessageBox.Show(
                    $"Could not find \"{ecWolfPath}\". " +
                    $"Verify that the path to ecwolf.exe in {ECWolfPathConfigurationFile} is correct.",
                    "Incorrect ecwolf.exe path",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            var wad = new WadFile();
            wad.Append(new Marker("MAP01"));
            wad.Append(new UwmfLump("TEXTMAP", uwmfMap));
            wad.Append(new Marker("ENDMAP"));
            wad.SaveTo(wadPath);

            Process.Start(
                ecWolfPath,
                $"--file \"{wadPath}\" --hard --nowait --tedlevel map01");
        }

        private void EditECWolfPath(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(ECWolfPathConfigurationFile))
            {
                File.CreateText(ECWolfPathConfigurationFile);
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = ECWolfPathConfigurationFile,
                UseShellExecute = true,
            });
        }
    }
}
