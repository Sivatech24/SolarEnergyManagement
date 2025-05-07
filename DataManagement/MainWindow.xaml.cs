using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
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
using CsvHelper.Configuration;
using CsvHelper;
using MathNet.Numerics.Statistics;
using Microsoft.Win32;

namespace DataManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataTable dataTable;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadCsv_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Select a CSV file"
            };

            if (dialog.ShowDialog() == true)
            {
                LoadCsv(dialog.FileName);
                DataGridView.ItemsSource = dataTable.DefaultView;
                AnalyzeData();
            }
        }

        private void LoadCsv(string path)
        {
            dataTable = new DataTable();

            var reader = new StreamReader(path);
            var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Trim(),
                MissingFieldFound = null
            });

            var dr = new CsvDataReader(csv);
            dataTable.Load(dr);
        }

        private void AnalyzeData()
        {
            if (dataTable == null || dataTable.Rows.Count == 0) return;

            var sb = new StringBuilder();
            sb.AppendLine("=== Data Analysis Summary ===");
            sb.AppendLine($"Total Rows: {dataTable.Rows.Count}");
            sb.AppendLine($"Total Columns: {dataTable.Columns.Count}");
            sb.AppendLine();

            foreach (DataColumn col in dataTable.Columns)
            {
                var colData = dataTable.Rows.OfType<DataRow>().Select(r => r[col]?.ToString()).ToList();

                int missingCount = colData.Count(x => string.IsNullOrWhiteSpace(x));
                sb.AppendLine($"Column: {col.ColumnName}");
                sb.AppendLine($"  Missing Values: {missingCount}");

                // Try parse numeric
                var numericValues = colData
                    .Where(x => double.TryParse(x, out _))
                    .Select(x => double.Parse(x))
                    .ToList();

                if (numericValues.Count > 0)
                {
                    var stats = new DescriptiveStatistics(numericValues);
                    sb.AppendLine($"  Count: {numericValues.Count}");
                    sb.AppendLine($"  Average: {stats.Mean:F2}");
                    sb.AppendLine($"  Min: {stats.Minimum}");
                    sb.AppendLine($"  Max: {stats.Maximum}");
                    sb.AppendLine($"  StdDev: {stats.StandardDeviation:F2}");
                }
                else
                {
                    sb.AppendLine("  Non-numeric column.");
                }

                sb.AppendLine();
            }

            AnalysisTextBox.Text = sb.ToString();
        }
    }
}
