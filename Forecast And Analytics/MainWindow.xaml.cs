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
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace Forecast_And_Analytics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Define a dictionary to hold the date and corresponding yield value
        private Dictionary<string, double> dailyYields = new Dictionary<string, double>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void LoadCsv_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog { Filter = "CSV files (*.csv)|*.csv" };

            if (dlg.ShowDialog() == true)
            {
                ProcessingBar.Visibility = Visibility.Visible;
                EstimatedTimeText.Text = "Processing...";
                ProcessingBar.Value = 0;

                Stopwatch sw = Stopwatch.StartNew();

                // Load CSV data asynchronously
                var loadedData = await Task.Run(() =>
                {
                    var lines = File.ReadAllLines(dlg.FileName).Skip(1);
                    var daily = new Dictionary<string, double>();

                    foreach (var line in lines)
                    {
                        var parts = line.Split(',');
                        if (parts.Length < 7) continue;

                        string dateStr = parts[0].Split(' ')[0]; // "15-05-2020"
                        if (!double.TryParse(parts[5], out double yield)) continue;

                        if (!daily.ContainsKey(dateStr))
                            daily[dateStr] = 0;

                        daily[dateStr] += yield;  // Sum up the daily yields for the same date
                    }

                    return daily.OrderBy(kvp => DateTime.ParseExact(kvp.Key, "dd-MM-yyyy", null)).ToList();
                });

                dailyYields = loadedData.ToDictionary(k => k.Key, v => v.Value);  // Assign to global variable

                var actualValues = dailyYields.Select(k => k.Value).ToList();
                var dateLabels = dailyYields.Select(k => k.Key).ToList();
                var forecast = PredictNextDay(actualValues);

                ShowChartWithForecast(actualValues, forecast, dateLabels);
                ExplainNextDayForecast(actualValues, forecast, dateLabels.Last());

                sw.Stop();
                EstimatedTimeText.Text = $"Done in {sw.ElapsedMilliseconds} ms";
                ProcessingBar.Visibility = Visibility.Collapsed;
            }
        }

        private double PredictNextDay(List<double> values)
        {
            int n = values.Count;
            if (n < 3) return values.Last();
            int window = Math.Min(5, n); // Take last 5 days for moving average
            return values.Skip(n - window).Take(window).Average();
        }

        private void ShowChartWithForecast(List<double> actual, double forecast, List<string> labels)
        {
            var forecastSeries = new List<double>(actual) { double.NaN, forecast };  // add forecast with a gap

            ForecastChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Actual",
                    Values = new ChartValues<double>(actual),
                    PointGeometry = DefaultGeometries.Circle,
                    StrokeThickness = 2
                },
                new LineSeries
                {
                    Title = "Forecast",
                    Values = new ChartValues<double>(forecastSeries),
                    PointGeometry = DefaultGeometries.Square,
                    StrokeDashArray = new DoubleCollection { 4 },
                    Stroke = Brushes.Red,
                    StrokeThickness = 2
                }
            };

            var fullLabels = new List<string>(labels) { "", "Next Day" };
            ForecastChart.AxisX.Clear();
            ForecastChart.AxisX.Add(new Axis
            {
                Labels = fullLabels,
                Title = "Date"
            });
        }

        private void ExplainNextDayForecast(List<double> actual, double forecast, string lastDate)
        {
            int n = actual.Count;
            double average = actual.Average();
            double last = actual.Last();
            double diff = forecast - last;

            ExplanationText.Text =
                $"📅 Last Available Date: {lastDate}\n" +
                $"📈 Days Analyzed: {n}\n" +
                $"⚡️ Last Day Yield: {last:F2} kWh\n" +
                $"📊 Average Yield (Last {Math.Min(5, n)} days): {actual.Skip(Math.Max(0, n - 5)).Average():F2} kWh\n" +
                $"🔮 Predicted Yield (Next Day): {forecast:F2} kWh\n" +
                $"📉 Change vs Last Day: {(diff >= 0 ? "+" : "")}{diff:F2} kWh\n" +
                $"🧠 This forecast uses a simple moving average of the last {Math.Min(5, n)} days.";
        }

        private async void PredictButton_Click(object sender, RoutedEventArgs e)
        {
            // Ensure data is loaded first
            if (dailyYields == null || !dailyYields.Any())
            {
                MessageBox.Show("Please load the CSV file first.");
                return;
            }

            int predictionDays = int.Parse((PredictionDaysComboBox.SelectedItem as ComboBoxItem).Content.ToString());

            ProcessingBar.Visibility = Visibility.Visible;
            EstimatedTimeText.Text = "Processing Prediction...";
            ProcessingBar.Value = 0;

            Stopwatch sw = Stopwatch.StartNew();

            // Predict the next 'n' days
            var actualValues = dailyYields.Select(k => k.Value).ToList();
            var dateLabels = dailyYields.Select(k => k.Key).ToList();
            var forecast = PredictNextDays(actualValues, predictionDays);

            // Display Chart with forecasted data
            ShowChartWithForecast(actualValues, forecast, dateLabels, predictionDays);

            // Display Explanation for Forecast
            ExplainNextDaysForecast(actualValues, forecast, dateLabels.Last(), predictionDays);

            sw.Stop();
            EstimatedTimeText.Text = $"Done in {sw.ElapsedMilliseconds} ms";
            ProcessingBar.Visibility = Visibility.Collapsed;
        }

        private List<double> PredictNextDays(List<double> actualValues, int predictionDays)
        {
            List<double> forecast = new List<double>(actualValues);

            // Get the last N days for averaging (to predict future values)
            int n = actualValues.Count;
            if (n < 3) return forecast;  // if not enough data, return as is

            var lastDays = actualValues.Skip(Math.Max(0, n - 5)).Take(5).ToList();
            double predictedValue = lastDays.Average(); // Using a moving average for prediction

            for (int i = 0; i < predictionDays; i++)
            {
                forecast.Add(predictedValue);  // Append predicted values for the requested days
            }

            return forecast;
        }

        private void ShowChartWithForecast(List<double> actual, List<double> forecast, List<string> labels, int predictionDays)
        {
            // Extend labels for forecasted days
            var extendedLabels = new List<string>(labels);
            for (int i = 1; i <= predictionDays; i++)
            {
                extendedLabels.Add($"Day {labels.Count + i}");
            }

            var forecastSeries = new List<double>(actual);
            forecastSeries.AddRange(forecast);  // Add the predicted values

            ForecastChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Actual",
                    Values = new ChartValues<double>(actual),
                    PointGeometry = DefaultGeometries.Circle,
                    StrokeThickness = 2
                },
                new LineSeries
                {
                    Title = "Forecast",
                    Values = new ChartValues<double>(forecastSeries),
                    PointGeometry = DefaultGeometries.Square,
                    StrokeDashArray = new DoubleCollection { 4 },
                    Stroke = Brushes.Red,
                    StrokeThickness = 2
                }
            };

            // Update X Axis labels
            ForecastChart.AxisX.Clear();
            ForecastChart.AxisX.Add(new Axis
            {
                Labels = extendedLabels,
                Title = "Day"
            });
        }

        private void ExplainNextDaysForecast(List<double> actual, List<double> forecast, string lastDate, int predictionDays)
        {
            int n = actual.Count;
            double last = actual.Last();
            double diff = forecast[0] - last;

            ExplanationText.Text =
                $"📅 Last Available Date: {lastDate}\n" +
                $"📈 Days Analyzed: {n}\n" +
                $"⚡️ Last Day Yield: {last:F2} kWh\n" +
                $"📊 Average Yield (Last {Math.Min(5, n)} days): {actual.Skip(Math.Max(0, n - 5)).Average():F2} kWh\n" +
                $"🔮 Predicted Yield for Next {predictionDays} Day(s):\n" +
                string.Join("\n", forecast.Skip(n).Take(predictionDays).Select((value, idx) => $"  Day {idx + 1}: {value:F2} kWh")) +
                $"\n📉 Change vs Last Day: {(diff >= 0 ? "+" : "")}{diff:F2} kWh\n" +
                $"🧠 This forecast uses a simple moving average of the last {Math.Min(5, n)} days.";
        }
    }
}
