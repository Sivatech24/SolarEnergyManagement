using CsvHelper;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public SeriesCollection Series { get; set; }
        public List<string> Labels { get; set; } = new List<string>();
        private ChartValues<double> PowerValues = new ChartValues<double>();

        private List<SolarRow> data = new List<SolarRow>();
        private int dataIndex = 0;
        private DispatcherTimer timer;
        private double minPower, maxPower;
        private double lat, lon;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Power (MW)",
                    Values = PowerValues,
                    PointGeometrySize = 0
                }
            };

            // Setup map
            MapControl.MapProvider = GMap.NET.MapProviders.BingSatelliteMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            MapControl.ShowCenter = false;
            MapControl.Zoom = 7;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
        }

        private void BrowseCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "CSV Files (*.csv)|*.csv" };
            if (dlg.ShowDialog() == true)
            {
                CsvFileNameText.Text = System.IO.Path.GetFileName(dlg.FileName);
                LoadData(dlg.FileName);
                StartAnimation();
            }
        }

        private void LoadData(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                data = csv.GetRecords<SolarRow>().ToList();
            }

            if (data.Count == 0)
            {
                MessageBox.Show("No data found in CSV.");
                return;
            }

            lat = data[0].latitude;
            lon = data[0].longitude;
            minPower = data.Min(r => r.power_mw);
            maxPower = data.Max(r => r.power_mw);

            // Reset chart and map
            PowerValues.Clear();
            Labels.Clear();
            dataIndex = 0;

            // Center map and clear all markers
            MapControl.Position = new PointLatLng(lat, lon);
            MapControl.Markers.Clear();
        }

        private void StartAnimation()
        {
            timer.Stop();
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (dataIndex >= data.Count)
            {
                timer.Stop();
                return;
            }

            var row = data[dataIndex];
            PowerValues.Add(row.power_mw);
            Labels.Add(row.local_time.ToString("MM/dd HH:mm"));

            // Normalize power for color
            double norm = (row.power_mw - minPower) / (maxPower - minPower + 1e-6);
            var color = GetHeatColor(norm);
            double size = 10; // Small fixed size for a point
            double opacity = 0.8;

            // Create a small circle marker at the exact site coordinates
            var ellipse = new Ellipse
            {
                Width = size,
                Height = size,
                Fill = new SolidColorBrush(color),
                Opacity = opacity,
                IsHitTestVisible = false
            };

            var marker = new GMapMarker(new PointLatLng(lat, lon))
            {
                Shape = ellipse,
                Offset = new System.Windows.Point(-size / 2, -size / 2) // Center the point
            };
            MapControl.Markers.Add(marker);

            OnPropertyChanged(nameof(Labels));
            dataIndex++;
        }

        // Heat color mapping: blue (low) -> green (medium) -> red (high), then blend with gray for medium brightness
        private Color GetHeatColor(double norm)
        {
            Color baseColor;
            if (norm < 0.5)
            {
                // Blue to Green
                double t = norm / 0.5;
                baseColor = Color.FromRgb(
                    (byte)(0),
                    (byte)(0 + (255) * t),
                    (byte)(255 - (255) * t)
                );
            }
            else
            {
                // Green to Red
                double t = (norm - 0.5) / 0.5;
                baseColor = Color.FromRgb(
                    (byte)(0 + (255) * t),
                    (byte)(255 - (255) * t),
                    0
                );
            }

            // Blend with medium gray to reduce brightness
            Color gray = Color.FromRgb(180, 180, 180);
            return BlendWithGray(baseColor, gray, 0.5); // 0.5 = 50% blend
        }

        // Helper to blend two colors
        private Color BlendWithGray(Color c1, Color c2, double t)
        {
            return Color.FromRgb(
                (byte)(c1.R * (1 - t) + c2.R * t),
                (byte)(c1.G * (1 - t) + c2.G * t),
                (byte)(c1.B * (1 - t) + c2.B * t)
            );
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // CSV row mapping
    public class SolarRow
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int year { get; set; }
        public string site_type { get; set; }
        public double capacity_mw { get; set; }
        public string resolution { get; set; }
        public int row_index { get; set; }
        public DateTime local_time { get; set; }
        public double power_mw { get; set; }
    }
}