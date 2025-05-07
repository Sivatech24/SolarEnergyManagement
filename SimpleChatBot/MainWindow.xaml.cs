using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using CsvHelper;
using CsvHelper.Configuration;

namespace WpfSolarBot
{
    public partial class MainWindow : Window
    {
        private List<SolarData> solarDataList;

        public MainWindow()
        {
            InitializeComponent();
            solarDataList = new List<SolarData>();
        }

        // Class to represent your CSV data
        public class SolarData
        {
            public string DATE_TIME { get; set; }
            public int PLANT_ID { get; set; }
            public string SOURCE_KEY { get; set; }
            public double DC_POWER { get; set; }
            public double AC_POWER { get; set; }
            public double DAILY_YIELD { get; set; }
            public double TOTAL_YIELD { get; set; }
        }

        // Load the CSV file
        private void LoadCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                using (var reader = new StreamReader(openFileDialog.FileName))
                using (var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
                {
                    solarDataList = csv.GetRecords<SolarData>().ToList();
                }
                BotResponse.Text = "CSV loaded successfully!";
            }
        }

        // Handle placeholder behavior (hide it when the user starts typing)
        private void UserInput_GotFocus(object sender, RoutedEventArgs e)
        {
            PlaceholderText.Visibility = Visibility.Collapsed;
        }

        private void UserInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserInput.Text))
            {
                PlaceholderText.Visibility = Visibility.Visible;
            }
        }

        // Process the user input and respond with a detailed explanation
        private void AskBot_Click(object sender, RoutedEventArgs e)
        {
            string query = UserInput.Text.ToLower();

            // Query handling based on user input
            if (query.Contains("average dc power"))
            {
                var averageDC = solarDataList.Average(x => x.DC_POWER);
                BotResponse.Text = $"You asked for the average DC Power. Here's how I calculated it:\n" +
                                   $"Sum of DC Power values: {solarDataList.Sum(x => x.DC_POWER)} watts\n" +
                                   $"Number of records: {solarDataList.Count}\n" +
                                   $"Average DC Power: {averageDC:F2} watts";
            }
            else if (query.Contains("average ac power"))
            {
                var averageAC = solarDataList.Average(x => x.AC_POWER);
                BotResponse.Text = $"You asked for the average AC Power. Here's how I calculated it:\n" +
                                   $"Sum of AC Power values: {solarDataList.Sum(x => x.AC_POWER)} watts\n" +
                                   $"Number of records: {solarDataList.Count}\n" +
                                   $"Average AC Power: {averageAC:F2} watts";
            }
            else if (query.Contains("average total yield"))
            {
                var averageTotalYield = solarDataList.Average(x => x.TOTAL_YIELD);
                BotResponse.Text = $"You asked for the average Total Yield. Here's how I calculated it:\n" +
                                   $"Sum of Total Yield values: {solarDataList.Sum(x => x.TOTAL_YIELD)} kWh\n" +
                                   $"Number of records: {solarDataList.Count}\n" +
                                   $"Average Total Yield: {averageTotalYield:F2} kWh";
            }
            else if (query.Contains("average daily yield"))
            {
                var averageDailyYield = solarDataList.Average(x => x.DAILY_YIELD);
                BotResponse.Text = $"You asked for the average Daily Yield. Here's how I calculated it:\n" +
                                   $"Sum of Daily Yield values: {solarDataList.Sum(x => x.DAILY_YIELD)} kWh\n" +
                                   $"Number of records: {solarDataList.Count}\n" +
                                   $"Average Daily Yield: {averageDailyYield:F2} kWh";
            }
            else if (query.Contains("maximum dc power"))
            {
                var maxDC = solarDataList.Max(x => x.DC_POWER);
                BotResponse.Text = $"You asked for the maximum DC Power. Here's how I calculated it:\n" +
                                   $"Maximum DC Power recorded in the data: {maxDC} watts";
            }
            else if (query.Contains("maximum ac power"))
            {
                var maxAC = solarDataList.Max(x => x.AC_POWER);
                BotResponse.Text = $"You asked for the maximum AC Power. Here's how I calculated it:\n" +
                                   $"Maximum AC Power recorded in the data: {maxAC} watts";
            }
            else if (query.Contains("maximum total yield"))
            {
                var maxTotalYield = solarDataList.Max(x => x.TOTAL_YIELD);
                BotResponse.Text = $"You asked for the maximum Total Yield. Here's how I calculated it:\n" +
                                   $"Maximum Total Yield recorded in the data: {maxTotalYield} kWh";
            }
            else if (query.Contains("maximum daily yield"))
            {
                var maxDailyYield = solarDataList.Max(x => x.DAILY_YIELD);
                BotResponse.Text = $"You asked for the maximum Daily Yield. Here's how I calculated it:\n" +
                                   $"Maximum Daily Yield recorded in the data: {maxDailyYield} kWh";
            }
            else if (query.Contains("minimum dc power"))
            {
                var minDC = solarDataList.Min(x => x.DC_POWER);
                BotResponse.Text = $"You asked for the minimum DC Power. Here's how I calculated it:\n" +
                                   $"Minimum DC Power recorded in the data: {minDC} watts";
            }
            else if (query.Contains("minimum ac power"))
            {
                var minAC = solarDataList.Min(x => x.AC_POWER);
                BotResponse.Text = $"You asked for the minimum AC Power. Here's how I calculated it:\n" +
                                   $"Minimum AC Power recorded in the data: {minAC} watts";
            }
            else if (query.Contains("minimum total yield"))
            {
                var minTotalYield = solarDataList.Min(x => x.TOTAL_YIELD);
                BotResponse.Text = $"You asked for the minimum Total Yield. Here's how I calculated it:\n" +
                                   $"Minimum Total Yield recorded in the data: {minTotalYield} kWh";
            }
            else if (query.Contains("minimum daily yield"))
            {
                var minDailyYield = solarDataList.Min(x => x.DAILY_YIELD);
                BotResponse.Text = $"You asked for the minimum Daily Yield. Here's how I calculated it:\n" +
                                   $"Minimum Daily Yield recorded in the data: {minDailyYield} kWh";
            }
            else if (query.Contains("total dc power"))
            {
                var totalDC = solarDataList.Sum(x => x.DC_POWER);
                BotResponse.Text = $"You asked for the total DC Power. Here's how I calculated it:\n" +
                                   $"Sum of all DC Power values: {totalDC} watts";
            }
            else if (query.Contains("total ac power"))
            {
                var totalAC = solarDataList.Sum(x => x.AC_POWER);
                BotResponse.Text = $"You asked for the total AC Power. Here's how I calculated it:\n" +
                                   $"Sum of all AC Power values: {totalAC} watts";
            }
            else if (query.Contains("total total yield"))
            {
                var totalTotalYield = solarDataList.Sum(x => x.TOTAL_YIELD);
                BotResponse.Text = $"You asked for the total Total Yield. Here's how I calculated it:\n" +
                                   $"Sum of all Total Yield values: {totalTotalYield} kWh";
            }
            else if (query.Contains("total daily yield"))
            {
                var totalDailyYield = solarDataList.Sum(x => x.DAILY_YIELD);
                BotResponse.Text = $"You asked for the total Daily Yield. Here's how I calculated it:\n" +
                                   $"Sum of all Daily Yield values: {totalDailyYield} kWh";
            }
            else if (query.Contains("average plant id"))
            {
                var averagePlantID = solarDataList.Average(x => x.PLANT_ID);
                BotResponse.Text = $"You asked for the average Plant ID. Here's how I calculated it:\n" +
                                   $"Sum of Plant IDs: {solarDataList.Sum(x => x.PLANT_ID)}\n" +
                                   $"Number of records: {solarDataList.Count}\n" +
                                   $"Average Plant ID: {averagePlantID}";
            }
            else if (query.Contains("average source key"))
            {
                var sourceKeys = solarDataList.Select(x => x.SOURCE_KEY).Distinct();
                BotResponse.Text = $"You asked for the average Source Key. Here's how I calculated it:\n" +
                                   $"There are {sourceKeys.Count()} unique source keys in the dataset.";
            }
            else if (query.Contains("count of records"))
            {
                var recordCount = solarDataList.Count;
                BotResponse.Text = $"You asked for the count of records. Here's how I calculated it:\n" +
                                   $"Total records in the dataset: {recordCount}";
            }
            else if (query.Contains("first record"))
            {
                var firstRecord = solarDataList.FirstOrDefault();
                BotResponse.Text = $"You asked for the first record. Here's the first entry:\n" +
                                   $"DC Power: {firstRecord?.DC_POWER} watts\nAC Power: {firstRecord?.AC_POWER} watts\n" +
                                   $"Date/Time: {firstRecord?.DATE_TIME}";
            }
            else if (query.Contains("last record"))
            {
                var lastRecord = solarDataList.LastOrDefault();
                BotResponse.Text = $"You asked for the last record. Here's the last entry:\n" +
                                   $"DC Power: {lastRecord?.DC_POWER} watts\nAC Power: {lastRecord?.AC_POWER} watts\n" +
                                   $"Date/Time: {lastRecord?.DATE_TIME}";
            }
            else if (query.Contains("dc power statistics"))
            {
                var dcPowerStats = new
                {
                    Max = solarDataList.Max(x => x.DC_POWER),
                    Min = solarDataList.Min(x => x.DC_POWER),
                    Avg = solarDataList.Average(x => x.DC_POWER),
                    Total = solarDataList.Sum(x => x.DC_POWER)
                };
                BotResponse.Text = $"You asked for the DC Power statistics. Here's the detailed calculation:\n" +
                                   $"Maximum DC Power: {dcPowerStats.Max} watts\n" +
                                   $"Minimum DC Power: {dcPowerStats.Min} watts\n" +
                                   $"Average DC Power: {dcPowerStats.Avg:F2} watts\n" +
                                   $"Total DC Power: {dcPowerStats.Total} watts";
            }
            else if (query.Contains("ac power statistics"))
            {
                var acPowerStats = new
                {
                    Max = solarDataList.Max(x => x.AC_POWER),
                    Min = solarDataList.Min(x => x.AC_POWER),
                    Avg = solarDataList.Average(x => x.AC_POWER),
                    Total = solarDataList.Sum(x => x.AC_POWER)
                };
                BotResponse.Text = $"You asked for the AC Power statistics. Here's the detailed calculation:\n" +
                                   $"Maximum AC Power: {acPowerStats.Max} watts\n" +
                                   $"Minimum AC Power: {acPowerStats.Min} watts\n" +
                                   $"Average AC Power: {acPowerStats.Avg:F2} watts\n" +
                                   $"Total AC Power: {acPowerStats.Total} watts";
            }
            else if (query.Contains("yield statistics"))
            {
                var yieldStats = new
                {
                    Max = solarDataList.Max(x => x.TOTAL_YIELD),
                    Min = solarDataList.Min(x => x.TOTAL_YIELD),
                    Avg = solarDataList.Average(x => x.TOTAL_YIELD),
                    Total = solarDataList.Sum(x => x.TOTAL_YIELD)
                };
                BotResponse.Text = $"You asked for the Yield statistics. Here's the detailed calculation:\n" +
                                   $"Maximum Total Yield: {yieldStats.Max} kWh\n" +
                                   $"Minimum Total Yield: {yieldStats.Min} kWh\n" +
                                   $"Average Total Yield: {yieldStats.Avg:F2} kWh\n" +
                                   $"Total Total Yield: {yieldStats.Total} kWh";
            }
            else
            {
                BotResponse.Text = "Sorry, I couldn't understand your query. Please ask for calculations related to DC Power, AC Power, or Yield.";
            }

        }
    }
}
