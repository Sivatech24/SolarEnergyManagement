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
using System.Diagnostics;
using System.IO;

namespace SolarEnergyManagment
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

        private void HomeImage_Loaded(object sender, RoutedEventArgs e)
        {
            RenderOptions.SetBitmapScalingMode(HomeImage, BitmapScalingMode.HighQuality);
            string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "dashboard.jpg");
            if (File.Exists(imagePath))
            {
                HomeImage.Source = new BitmapImage(new Uri(imagePath));
            }
            else
            {
                MessageBox.Show("Image file not found:\n" + imagePath, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event handler to loop the video when it ends
        /*private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string videoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Video", "2249554-uhd_3840_2160_24fps.mp4");
            if (File.Exists(videoPath))
            {
                HomeVideo.Source = new Uri(videoPath);
                HomeVideo.Play();
            }
            else
            {
                MessageBox.Show("Video file not found:\n" + videoPath, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void HomeVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            HomeVideo.Position = TimeSpan.Zero;
            HomeVideo.Play();
        }

        */

        // Event handler for launching the app (you'll need to implement this method)
        private void LaunchApp_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string appPath = button.Tag.ToString();  // Get the path from the button's Tag
                System.Diagnostics.Process.Start(appPath);  // Launch the application
            }
        }

        private void OpenSolarDataAnalytics_Click(object sender, RoutedEventArgs e)
        {
            StartApp(@"C:\Users\tech\Documents\Visual Studio Works\SolarEnergyManagment\SolarDataAnalytics\SolarDataAnalytics.exe");
        }

        private void OpenChatBot_Click(object sender, RoutedEventArgs e)
        {
            StartApp(@"C:\Users\tech\Documents\Visual Studio Works\SolarEnergyManagment\SimpleChatBot\SimpleChatBot.exe");
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            StartApp(@"C:\Users\tech\Documents\Visual Studio Works\SolarEnergyManagment\Settings\Settings.exe");
        }

        private void OpenInverterFailurePredicting_Click(object sender, RoutedEventArgs e)
        {
            StartApp(@"C:\Users\tech\Documents\Visual Studio Works\SolarEnergyManagment\Inverter Failure Predicting\Inverter Failure Predicting.exe");
        }

        private void OpenForecastAnalytics_Click(object sender, RoutedEventArgs e)
        {
            StartApp(@"C:\Users\tech\Documents\Visual Studio Works\SolarEnergyManagment\Forecast And Analytics\Forecast And Analytics.exe");
        }

        private void OpenDataManagement_Click(object sender, RoutedEventArgs e)
        {
            StartApp(@"C:\Users\tech\Documents\Visual Studio Works\SolarEnergyManagment\DataManagement\DataManagement.exe");
        }

        private void OpenAboutPage_Click(object sender, RoutedEventArgs e)
        {
            StartApp(@"C:\Users\tech\Documents\Visual Studio Works\SolarEnergyManagment\AboutPage\AboutPage.exe");
        }

        private void OpenRealTime_Click(object sender, RoutedEventArgs e)
        {
            StartApp(@"C:\Users\tech\Documents\Visual Studio Works\SolarEnergyManagment\RealTime\RealTimeRender.exe");
        }

        private void StartApp(string path)
        {
            if (File.Exists(path))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("Application not found:\n" + path, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenUrl_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string url)
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to open URL:\n" + url + "\n\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
