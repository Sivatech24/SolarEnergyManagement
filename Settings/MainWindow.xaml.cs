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

namespace Settings
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoadSettings();
        }

        private void LoadSettings()
        {
            // For example, load some settings from config or database
            AppNameTextBox.Text = "SolarApp";
            VersionTextBox.Text = "1.0.0";
            DataSourceComboBox.SelectedIndex = 0; // Default to CSV File
            AlgorithmComboBox.SelectedIndex = 0; // Default to Linear Regression
            AccuracySlider.Value = 75; // Default accuracy
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Save settings here
            string appName = AppNameTextBox.Text;
            string version = VersionTextBox.Text;
            string dataSource = ((ComboBoxItem)DataSourceComboBox.SelectedItem).Content.ToString();
            string algorithm = ((ComboBoxItem)AlgorithmComboBox.SelectedItem).Content.ToString();
            double accuracy = AccuracySlider.Value;

            // Code to save these settings, for example, to a file or database
        }
    }
}
