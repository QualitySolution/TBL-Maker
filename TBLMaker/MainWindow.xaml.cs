using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace TBLMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int NumberColumn = 0;
        private const int ParameterColumn = 1;
        private const int FileColumn = 2;
        private const int SizeColumn = 3;
        private const int ButtonColumn = 4;

        private TBLFile _file = null;
        public MainWindow()
        {
            InitializeComponent();
            MenuOpen.Click += MenuOpenOnClick;
            MenuSave.Click += MenuSaveOnClick;
            MenuSave.IsEnabled = false;
            MenuSaveAs.Click += MenuSaveAsOnClick;
            MenuSaveAs.IsEnabled = false;
            MenuClose.Click += (sender, args) => Application.Current.Shutdown(0);
        }

        private void MenuSaveAsOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var saveDlg = new SaveFileDialog { Filter = "Файлы TBL (*.tbl)|*.tbl" };
            if (saveDlg.ShowDialog() == true)
            {
                _file.Path = saveDlg.FileName;
                FillFileStructure();
                TBLWriter.Write(_file);
                this.Title = string.Format("TBL Maker - {0}", saveDlg.FileName.Substring(saveDlg.FileName.LastIndexOf("\\", StringComparison.Ordinal) + 1));
            }
        }

        private void MenuSaveOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            FillFileStructure();
            TBLWriter.Write(_file);
        }

        private void MenuOpenOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var openDlg = new OpenFileDialog { Multiselect = false, Filter = "Файлы TBL (*.tbl)|*.tbl" };
            if (openDlg.ShowDialog() == true)
                _file = TBLParser.Parse(openDlg.FileName);
            else
                return;
            Grid1.Children.Clear();
            Grid1.RowDefinitions.Clear();
            CreateHeader();
            this.Title = string.Format("TBL Maker - {0}", openDlg.FileName.Substring(openDlg.FileName.LastIndexOf("\\", StringComparison.Ordinal) + 1));

            foreach (var record in _file.ParsedTBLRecords)
            {
                var row = new RowDefinition { MinHeight = 30, MaxHeight = 30};
                Grid1.RowDefinitions.Add(row);

                var lb1 = new Label();
                Grid.SetColumn(lb1, NumberColumn);
                Grid.SetRow(lb1, Grid1.RowDefinitions.Count - 1);
                lb1.Content = record.FileNumber;

                var tb1 = new TextBox();
                Grid.SetColumn(tb1, ParameterColumn);
                Grid.SetRow(tb1, Grid1.RowDefinitions.Count - 1);
                tb1.Text = record.ParameterName;
                tb1.Margin = new Thickness(5);
                tb1.MaxHeight = 20;

                var tb2 = new TextBox();
                Grid.SetColumn(tb2, FileColumn);
                Grid.SetRow(tb2, Grid1.RowDefinitions.Count - 1);
                tb2.Text = record.FileName;
                tb1.Margin = new Thickness(5);
                tb2.MaxHeight = 20;

                var lb2 = new Label();
                Grid.SetColumn(lb2, SizeColumn);
                Grid.SetRow(lb2, Grid1.RowDefinitions.Count - 1);
                lb2.Content = record.FileSize;

                var img = new Image {Source = new BitmapImage(new Uri("Resources\\open.png", UriKind.Relative))};
                var stackPnl = new StackPanel {Orientation = Orientation.Horizontal};
                stackPnl.Children.Add(img);

                var btn = new Button();
                Grid.SetColumn(btn, ButtonColumn);
                Grid.SetRow(btn, Grid1.RowDefinitions.Count - 1);
                btn.Click += OpenBinFileButtonOnClick;
                btn.Content = stackPnl;
                btn.Margin = new Thickness(5);

                Grid1.Children.Add(lb1);
                Grid1.Children.Add(tb1);
                Grid1.Children.Add(tb2);
                Grid1.Children.Add(lb2);
                Grid1.Children.Add(btn);
            }
            this.Height = Grid1.RowDefinitions.Count*30 + 80;
            MenuSave.IsEnabled = true;
            MenuSaveAs.IsEnabled = true;
        }

        private void OpenBinFileButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var btn = sender as UIElement;
            if (btn != null)
            {
                var row = Grid.GetRow(btn);
                var openDlg = new OpenFileDialog { Multiselect = false, Filter = "Файлы BIN (*.bin)|*.bin" };
                if (openDlg.ShowDialog() == true)
                {
                    FileInfo info = new FileInfo(openDlg.FileName);

                    var tbName = Grid1.Children.Cast<UIElement>()
                    .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == FileColumn) as TextBox;
                    if (tbName != null) tbName.Text = info.Name;

                    string fileSize = Convert.ToString(info.Length, 16).PadLeft(6, '0');
                    var first = fileSize.Substring(0, 2);
                    var second = fileSize.Substring(2, 2);
                    var third = fileSize.Substring(4, 2);
                    fileSize = third + second + first;
                    var lbSize = Grid1.Children.Cast<UIElement>()
                        .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == SizeColumn) as Label;
                    if (lbSize != null) lbSize.Content = fileSize;
                }
            }
        }

        private void CreateHeader()
        {
            var row = new RowDefinition { MinHeight = 30 };
            Grid1.RowDefinitions.Add(row);

            var lb1 = new Label();
            Grid.SetColumn(lb1, NumberColumn);
            Grid.SetRow(lb1, Grid1.RowDefinitions.Count - 1);
            lb1.Content = "№";
            lb1.FontWeight = FontWeights.Bold;

            var lb2 = new Label();
            Grid.SetColumn(lb2, ParameterColumn);
            Grid.SetRow(lb2, Grid1.RowDefinitions.Count - 1);
            lb2.Content = "Название параметра";
            lb2.FontWeight = FontWeights.Bold;

            var lb3 = new Label();
            Grid.SetColumn(lb3, FileColumn);
            Grid.SetRow(lb3, Grid1.RowDefinitions.Count - 1);
            lb3.Content = "Название файла";
            lb3.FontWeight = FontWeights.Bold;

            var lb4 = new Label();
            Grid.SetColumn(lb4, SizeColumn);
            Grid.SetRow(lb4, Grid1.RowDefinitions.Count - 1);
            lb4.Content = "Размер";
            lb4.FontWeight = FontWeights.Bold;

            Grid1.Children.Add(lb1);
            Grid1.Children.Add(lb2);
            Grid1.Children.Add(lb3);
            Grid1.Children.Add(lb4);

        }

        private void FillFileStructure()
        {
            for (int i = 1; i < Grid1.RowDefinitions.Count; i++)
            {
                var tbParameter = Grid1.Children.Cast<UIElement>()
                    .First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == ParameterColumn) as TextBox;
                if (tbParameter != null) _file.ParsedTBLRecords[i - 1].ParameterName = tbParameter.Text;

                var tbName = Grid1.Children.Cast<UIElement>()
                    .First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == FileColumn) as TextBox;
                if (tbName != null) _file.ParsedTBLRecords[i - 1].FileName = tbName.Text;

                var lbSize = Grid1.Children.Cast<UIElement>()
                    .First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == SizeColumn) as Label;
                if (lbSize != null) _file.ParsedTBLRecords[i - 1].FileSize = lbSize.Content as string;
            }
        }
    }
}
