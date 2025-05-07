using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CsvHelper;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace InverterFailureDetection
{
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private async void LoadDataButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog { Filter = "CSV Files|*.csv" };
            if (dialog.ShowDialog() != true) return;

            ProgressBar.Value = 0;
            FailureListBox.ItemsSource = null;
            FailureCountLabel.Content = "Loading...";

            var filePath = dialog.FileName;
            var data = await Task.Run(() => LoadCsv(filePath));
            ProgressBar.Value = 30;

            var failures = DetectFailures(data);
            ProgressBar.Value = 60;

            FailureCountLabel.Content = $"Failures detected: {failures.Count}";
            FailureListBox.ItemsSource = failures.Select(f =>
                $"Time: {f.DATE_TIME} | Source: {f.SOURCE_KEY} | Reason: DC={f.DC_POWER}, AC={f.AC_POWER}");

            var plot = CreatePlot(data, failures);
            PlotView.Model = plot;

            ProgressBar.Value = 100;
        }

        private List<InverterData> LoadCsv(string filePath)
        {
            var records = new List<InverterData>();
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new InverterData
                    {
                        DATE_TIME = csv.GetField("DATE_TIME"),
                        PLANT_ID = int.Parse(csv.GetField("PLANT_ID")),
                        SOURCE_KEY = csv.GetField("SOURCE_KEY"),
                        DC_POWER = double.Parse(csv.GetField("DC_POWER")),
                        AC_POWER = double.Parse(csv.GetField("AC_POWER")),
                        DAILY_YIELD = double.Parse(csv.GetField("DAILY_YIELD")),
                        TOTAL_YIELD = double.Parse(csv.GetField("TOTAL_YIELD"))
                    };
                    records.Add(record);
                }
            }
            return records;
        }

        private List<InverterData> DetectFailures(List<InverterData> data)
        {
            return data
                .Where(d => d.DC_POWER < 1 && d.AC_POWER < 1)
                .ToList();
        }

        private PlotModel CreatePlot(List<InverterData> data, List<InverterData> failures)
        {
            var model = new PlotModel { Title = "DC and AC Power" };
            var timeAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Record Index"
            };
            model.Axes.Add(timeAxis);

            var powerAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Power (W)"
            };
            model.Axes.Add(powerAxis);

            var dcSeries = new LineSeries { Title = "DC Power", Color = OxyColors.Blue };
            var acSeries = new LineSeries { Title = "AC Power", Color = OxyColors.Orange };

            for (int i = 0; i < data.Count; i++)
            {
                dcSeries.Points.Add(new DataPoint(i, data[i].DC_POWER));
                acSeries.Points.Add(new DataPoint(i, data[i].AC_POWER));
            }

            model.Series.Add(dcSeries);
            model.Series.Add(acSeries);

            var failureMarkers = new ScatterSeries
            {
                MarkerType = MarkerType.Cross,
                MarkerSize = 5,
                MarkerFill = OxyColors.Red,
                Title = "Failures"
            };

            foreach (var failure in failures)
            {
                var index = data.IndexOf(failure);
                failureMarkers.Points.Add(new ScatterPoint(index, failure.DC_POWER));
            }

            model.Series.Add(failureMarkers);

            return model;
        }
    }

    public class InverterData
    {
        public string DATE_TIME { get; set; }
        public int PLANT_ID { get; set; }
        public string SOURCE_KEY { get; set; }
        public double DC_POWER { get; set; }
        public double AC_POWER { get; set; }
        public double DAILY_YIELD { get; set; }
        public double TOTAL_YIELD { get; set; }
    }
}
