using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Wpf;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CsvHelper;
using System.Globalization;

namespace DataVisualizerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void BrowseCsv_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var records = LoadCsv(openFileDialog.FileName);
                if (records.Count > 0)
                    DrawGraphs(records);
            }
        }

        private List<Dictionary<string, double>> LoadCsv(string filePath)
        {
            var data = new List<Dictionary<string, double>>();

            var reader = new StreamReader(filePath);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<dynamic>().ToList();

            foreach (var row in records)
            {
                var dict = new Dictionary<string, double>();
                foreach (var kv in (IDictionary<string, object>)row)
                {
                    if (double.TryParse(kv.Value?.ToString(), out double val))
                        dict[kv.Key] = val;
                }
                data.Add(dict);
            }

            return data;
        }

        private void DrawGraphs(List<Dictionary<string, double>> data)
        {
            ChartContainer.Children.Clear();

            if (data.Count == 0)
                return;

            // Get all unique keys (column headers)
            var keys = data[0].Keys.ToList();

            foreach (var key in keys)
            {
                var model = new PlotModel { Title = key };
                var series = new LineSeries();

                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].ContainsKey(key))
                        series.Points.Add(new DataPoint(i, data[i][key]));
                }

                model.Series.Add(series);
                model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Index" });
                model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = key });

                var chart = new PlotView
                {
                    Model = model,
                    Height = 250,
                    Margin = new Thickness(10)
                };

                ChartContainer.Children.Add(chart);
            }
        }
    }
}
