using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using CsvHelper;
using CsvHelper.Configuration;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.ObjectModel;
using System.Data;

namespace SolarEnergyAnalytics
{
    public partial class MainWindow : Window
    {
        private List<SolarData> _solarDataList = new List<SolarData>();
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void LoadDefaultDataButton_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://raw.githubusercontent.com/Sivatech24/AI-Driven-Solar-Energy-Management-Forecasting-Optimization-Fault-Detection/488502dd3c4ac9de6f84641cf2dda34acbafc811/SolarWorks/NoteBook/Plant_1_Generation_Data.csv";
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Plant_1_Generation_Data.csv");

            try
            {
                if (!File.Exists(filePath))
                {
                    var webClient = new System.Net.WebClient();
                    await webClient.DownloadFileTaskAsync(new Uri(url), filePath);
                }

                await LoadAndRenderData(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load default dataset:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void BrowseDatasetButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "CSV files (*.csv)|*.csv";

            if (dialog.ShowDialog() == true)
            {
                await LoadAndRenderData(dialog.FileName);
            }
        }
        private async Task LoadAndRenderData(string filePath)
        {
            DataLoadProgressBar.Visibility = Visibility.Visible;
            DataLoadProgressBar.IsIndeterminate = true;
            ChartRenderProgressBar.Visibility = Visibility.Visible;

            try
            {
                await Task.Run(() =>
                {
                    List<SolarData> records = null;

                    var reader = new StreamReader(filePath);
                    var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
                    csv.Context.RegisterClassMap<SolarDataMap>();
                    records = csv.GetRecords<SolarData>().ToList();
                    reader.Close();

                    Dispatcher.Invoke(() =>
                    {
                        _solarDataList = records;

                        // Display all records without taking any count from ComboBox
                        DataGridView.ItemsSource = records;

                        UpdateCharts();
                    });
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading file:\n" + ex.Message, "Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                DataLoadProgressBar.Visibility = Visibility.Collapsed;
                ChartRenderProgressBar.Visibility = Visibility.Collapsed;
            }
        }
        private void UpdateCharts()
        {
            UpdateLineChart();
            UpdateBarChart();
            UpdatePieChart();
            CalculateAndPlotLosses();
            PlotDcPowerGroupedByTime();
        }
        private void UpdateLineChart()
        {
            var grouped = _solarDataList
                .GroupBy(x => x.Date.Date)
                .Select(g => new
                {
                    DateTime = g.Key,
                    DailyYield = g.Sum(x => x.DailyYield)
                })
                .OrderBy(x => x.DateTime)
                .Take(1000)
                .ToList();

            LineChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Daily Yield",
                    Values = new ChartValues<double>(grouped.Select(x => x.DailyYield)),
                    PointGeometrySize = 4,
                    StrokeThickness = 2
                }
            };

            LineChart.AxisX[0].Labels = grouped.Select(x => x.DateTime.ToString("dd-MM")).ToList();
            LineChart.AxisX[0].Title = "Date";
            LineChart.AxisY[0].Title = "kW";
        }
        private void UpdateBarChart()
        {
            var grouped = _solarDataList
                .GroupBy(d => d.Date.Date)
                .ToList();

            BarChart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Avg DC Power",
                    Values = new ChartValues<double>(grouped.Select(g => g.Average(x => x.DcPower)))
                },
                new ColumnSeries
                {
                    Title = "Avg AC Power",
                    Values = new ChartValues<double>(grouped.Select(g => g.Average(x => x.AcPower)))
                }
            };

            BarChart.AxisX[0].Labels = grouped.Select(g => g.Key.ToString("dd-MM")).ToList();
        }
        private void UpdatePieChart()
        {
            var topSources = _solarDataList
                .GroupBy(x => x.SourceKey)
                .Select(g => new { g.Key, Total = g.Sum(x => x.AcPower) })
                .OrderByDescending(x => x.Total)
                .Take(5);

            PieChart.Series = new SeriesCollection();
            foreach (var source in topSources)
            {
                PieChart.Series.Add(new PieSeries
                {
                    Title = source.Key,
                    Values = new ChartValues<double> { source.Total },
                    DataLabels = true
                });
            }
        }
        private void CalculateAndPlotLosses()
        {
            // Copy the data
            var loss = _solarDataList.ToList();  // Corrected line to copy the list

            // Create a new 'day' column with only the date part
            var lossWithDay = loss.Select(x => new
            {
                x.Date,
                x.DcPower,
                x.AcPower,
                Day = x.Date.Date
            }).ToList();

            // Group by 'day' and sum only numeric columns
            var groupedLoss = lossWithDay
                .GroupBy(x => x.Day)
                .Select(g => new
                {
                    Day = g.Key,
                    DcPower = g.Sum(x => x.DcPower),
                    AcPower = g.Sum(x => x.AcPower)
                })
                .ToList();

            // Calculate the percentage of DC power converted to AC power
            var losses = groupedLoss
                .Select(g => new
                {
                    g.Day,
                    Losses = (g.AcPower / g.DcPower) * 100
                })
                .ToList();

            // Bind the data to the LossChart
            LossChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                Title = "DC to AC Power Conversion",
                Values = new ChartValues<double>(losses.Select(l => l.Losses)),
                PointGeometrySize = 5,
                StrokeThickness = 2
                }
            };

            // Set the X Axis labels
            LossChart.AxisX[0].Labels = losses.Select(l => l.Day.ToString("dd-MM-yyyy")).ToList();
        }
        private async void PlotDcPowerGroupedByTime()
        {
            // Check if there is any data to process
            if (_solarDataList == null || !_solarDataList.Any())
            {
                MessageBox.Show("No data available for plotting DC power grouped by time.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Run the processing in the background thread to avoid blocking the UI thread
            await Task.Run(() =>
            {
                // Process data in the background thread
                var dcGen = _solarDataList.ToList();

                // Extract time part from DateTime for the data
                var dcGenWithTime = dcGen.Select(x => new
                {
                    x.DcPower,
                    x.SourceKey,
                    Time = x.Date.TimeOfDay // Extract only the time part
                }).ToList();

                // Group by time and source key and get the average DC Power for each group
                var groupedData = dcGenWithTime
                    .GroupBy(x => new { x.Time, x.SourceKey })
                    .Select(g => new
                    {
                        g.Key.Time,
                        g.Key.SourceKey,
                        AvgDcPower = g.Average(x => x.DcPower)
                    })
                    .OrderBy(x => x.Time)
                    .ToList();

                // Split the grouped data into two parts: First 11 sources and Last 11 sources
                var first11Sources = groupedData.Where(x => Array.IndexOf(groupedData.Select(g => g.SourceKey).Distinct().ToArray(), x.SourceKey) < 11).ToList();
                var last11Sources = groupedData.Where(x => Array.IndexOf(groupedData.Select(g => g.SourceKey).Distinct().ToArray(), x.SourceKey) >= 11).ToList();

                // Update the UI on the main thread
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    // Clear existing chart data if any
                    DcPowerChart1.Series.Clear();
                    DcPowerChart2.Series.Clear();

                    // Create LineSeries for the first 11 sources
                    foreach (var source in first11Sources.GroupBy(x => x.SourceKey))
                    {
                        var series = new LineSeries
                        {
                            Title = source.Key,
                            Values = new ChartValues<double>(source.Select(x => x.AvgDcPower).ToList()),
                            PointGeometrySize = 5,
                            StrokeThickness = 2
                        };
                        DcPowerChart1.Series.Add(series);
                    }

                    // Create LineSeries for the last 11 sources
                    foreach (var source in last11Sources.GroupBy(x => x.SourceKey))
                    {
                        var series = new LineSeries
                        {
                            Title = source.Key,
                            Values = new ChartValues<double>(source.Select(x => x.AvgDcPower).ToList()),
                            PointGeometrySize = 5,
                            StrokeThickness = 2
                        };
                        DcPowerChart2.Series.Add(series);
                    }

                    // Set the X Axis labels (time)
                    var timeLabels = groupedData.Select(x => x.Time.ToString(@"hh\:mm")).Distinct().ToList();
                    DcPowerChart1.AxisX[0].Labels = timeLabels;
                    DcPowerChart2.AxisX[0].Labels = timeLabels;

                    // Set chart titles and axis labels
                    DcPowerChart1.AxisY[0].Title = "DC POWER (kW)";
                    DcPowerChart1.AxisX[0].Title = "Time of Day";
                    DcPowerChart2.AxisY[0].Title = "DC POWER (kW)";
                    DcPowerChart2.AxisX[0].Title = "Time of Day";
                }));
            });
        }
    }
    public class SolarData
    {
        public DateTime Date { get; set; }
        public string PlantId { get; set; }
        public string SourceKey { get; set; }
        public double DcPower { get; set; }
        public double AcPower { get; set; }
        public double DailyYield { get; set; }
        public double TotalYield { get; set; }
        public double ModuleTemperature { get; set; }
        public double Irradiation { get; set; }
        public double AmbientTemperature { get; set; }
    }
    public sealed class SolarDataMap : ClassMap<SolarData>
    {
        public SolarDataMap()
        {
            Map(m => m.Date).Name("DATE_TIME").TypeConverterOption.Format("dd-MM-yyyy HH:mm");
            Map(m => m.PlantId).Name("PLANT_ID");
            Map(m => m.SourceKey).Name("SOURCE_KEY");
            Map(m => m.DcPower).Name("DC_POWER");
            Map(m => m.AcPower).Name("AC_POWER");
            Map(m => m.DailyYield).Name("DAILY_YIELD");
            Map(m => m.TotalYield).Name("TOTAL_YIELD");
        }
    }
}
